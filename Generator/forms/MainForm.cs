﻿using CombinationalCircuitDatabaseGenerator.forms;
using DataBaseGenerators;
using Newtonsoft.Json.Linq;
using Tulpep.NotificationWindow;
using Properties;
using source;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using CsvHelper;
using System.Text;
using Circuits;

namespace CombinationalCircuitDatabaseGenerator
{
    public partial class MainForm : Form
    {
        private List<DataBaseGeneratorParameters> parameters;
        private int counter;
        private DataBaseGenerator generator;
        private Settings settings;

        private Stopwatch stopWatch;

        private PopupNotifier popup = null;

        private CircuitForm cf;
        private NewGenerator ng;
        private bool isGeneratingRunning;
        public MainForm()
        {
            InitializeComponent();
            isGeneratingRunning = false;
            counter = 0;
            List<DataBaseGeneratorParameters> lstGenerators = new List<DataBaseGeneratorParameters>();

            cmbbxGenerationType.Items.Clear();
            cmbbxGenerationType.Items.AddRange(Enum.GetValues(typeof(GenerationTypes))
                .Cast<GenerationTypes>()
                .Select(v => v.ToString())
                .ToArray());            
            cmbbxGenerationType.SelectedIndex = 0;

            parameters = new List<DataBaseGeneratorParameters>();
            UpdateCircuitsList();
        }
                
        private bool Contains(TreeNode main, TreeNode node)
        {
            if (main == null)
                return false;
            if (main.Nodes.Contains(node) || main == node)
                return true;
            else
                foreach (TreeNode child in main.Nodes)
                    if (Contains(child, node))
                        return true;
            return false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (trvwGenerators.SelectedNode != null)
            {
                foreach (TreeNode node in trvwGenerators.Nodes)
                    if (Contains(node, trvwGenerators.SelectedNode))
                    {
                        parameters.RemoveAt(trvwGenerators.Nodes.IndexOf(node));
                        trvwGenerators.Nodes.Remove(node);
                        break;
                    }
            }
        }

        private TreeNode AddNode(string s)
        {
            TreeNode node = new TreeNode(s);
            return node;
        }
        private TreeNode AddNode(string s, int n)
        {
            TreeNode node = new TreeNode(s);
            node.Nodes.Add(n.ToString());
            return node;
        }
        private TreeNode AddNode(string s, double d)
        {
            TreeNode node = new TreeNode(s);
            string specifier = "F";
            CultureInfo culture = CultureInfo.CurrentCulture;
            node.Nodes.Add(d.ToString(specifier, culture));
            return node;
        }
        private TreeNode AddNode(string s, string s2)
        {
            TreeNode node = new TreeNode(s);
            node.Nodes.Add(s2);
            return node;
        }

        private TreeNode AddNode(string s, bool f)
        {
            TreeNode node = new TreeNode(s);
            node.Nodes.Add(f ? "True" : "False");
            return node;
        }

        private TreeNode AddMainNodeGenerators(DataBaseGeneratorParameters param)
        {
            TreeNode node = new TreeNode(counter++.ToString());

            node.Nodes.Add(AddNode("Минимальное количество входов", param.minInputs));
            node.Nodes.Add(AddNode("Максимальное количество входов", param.maxInputs));
            node.Nodes.Add(AddNode("Минимальное количество выходов", param.minOutputs));
            node.Nodes.Add(AddNode("Максимальное количество выходов", param.maxOutputs));
            node.Nodes.Add(AddNode("Количество повторений", param.eachIteration));
            node.Nodes.Add(AddNode("Тип генерации", param.generationTypes.ToString()));
            TreeNode node2 = new TreeNode("Параметры генерации");

            if (param.generationTypes == GenerationTypes.FromRandomTruthTable)
            {
                node2.Nodes.Add(AddNode("Ограничение генерации", param.generationParameters.cnfFromTruthTableParameters.generateLimitation));
                node2.Nodes.Add(AddNode("CNFF", param.generationParameters.cnfFromTruthTableParameters.CNFF));
                node2.Nodes.Add(AddNode("CNFT", param.generationParameters.cnfFromTruthTableParameters.CNFT));
            }

            if (param.generationTypes == GenerationTypes.RandLevel)
            {
                node2.Nodes.Add(AddNode("Максимальное количество уровней", param.generationParameters.generatorRandLevelParameters.maxLevel));
                node2.Nodes.Add(AddNode("Максимальное количество логических элементов", param.generationParameters.generatorRandLevelParameters.maxElements));
            }

            if (param.generationTypes == GenerationTypes.NumOperation)
            {
                node2.Nodes.Add(AddNode("Оставлять выходы пустыми", param.generationParameters.generatorNumOperationParameters.leaveEmptyOut));
                foreach (var v in param.generationParameters.generatorNumOperationParameters.logicOper)
                    if (v.Value != 0)
                        node2.Nodes.Add(AddNode(v.Key, v.Value));

            }

            if (param.generationTypes == GenerationTypes.Genetic)
            {
                node2.Nodes.Add(AddNode("Размер популяции", param.generationParameters.geneticParameters.populationSize));
                node2.Nodes.Add(AddNode("Количество циклов повторения", param.generationParameters.geneticParameters.numOfCycles));
                node2.Nodes.Add(AddNode("Индекс завершения", param.generationParameters.geneticParameters.keyEndProcessIndex));

                TreeNode recombination = new TreeNode("Воспроизведение");
                TreeNode parents = new TreeNode("Отбор родителей");
                parents.Nodes.Add(AddNode("Тип отбора родителей", param.generationParameters.geneticParameters.RecombinationParameter.ParentsParameter.ParentsType.ToString()));
                parents.Nodes.Add(AddNode("Размер турнира", param.generationParameters.geneticParameters.RecombinationParameter.ParentsParameter.TournematnNumber));
                recombination.Nodes.Add(parents);
                recombination.Nodes.Add(AddNode("Тип воспроизведения", param.generationParameters.geneticParameters.RecombinationParameter.RecombinationType.ToString()));
                recombination.Nodes.Add(AddNode("refPoints", param.generationParameters.geneticParameters.RecombinationParameter.refPoints));
                recombination.Nodes.Add(AddNode("maskProbability", param.generationParameters.geneticParameters.RecombinationParameter.maskProbability));
                recombination.Nodes.Add(AddNode("recombinationNumber", param.generationParameters.geneticParameters.RecombinationParameter.recombinationNumber));

                TreeNode mutation = new TreeNode("Мутация");
                mutation.Nodes.Add(AddNode("Тип мутации", param.generationParameters.geneticParameters.MutationParameter.MutationType.ToString()));
                mutation.Nodes.Add(AddNode("probabilityGen", param.generationParameters.geneticParameters.MutationParameter.probabilityGen));
                mutation.Nodes.Add(AddNode("exchangeType", param.generationParameters.geneticParameters.MutationParameter.exchangeType));
                mutation.Nodes.Add(AddNode("probabilityTruthTable", param.generationParameters.geneticParameters.MutationParameter.probabilityTruthTable));

                TreeNode selection = new TreeNode("Отбор");
                selection.Nodes.Add(AddNode("Тип отбора", param.generationParameters.geneticParameters.SelectionParameter.SelectionType.ToString()));
                selection.Nodes.Add(AddNode("Количество выживших", param.generationParameters.geneticParameters.SelectionParameter.numOfSurvivors));

                node2.Nodes.Add(recombination);
                node2.Nodes.Add(mutation);
                node2.Nodes.Add(selection);
            }


            node.Nodes.Add(node2);
            return node;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            GenerationTypes gp = AuxiliaryMethods.ToEnum<GenerationTypes>(cmbbxGenerationType.SelectedItem.ToString());
            NewGenerator f = new NewGenerator(gp);
            DialogResult res = f.ShowDialog();            

            if (res == DialogResult.OK)
            {
                DataBaseGeneratorParameters param = f.dataBaseGeneratorParameters;
                parameters.Add(param);
                trvwGenerators.Nodes.Add(AddMainNodeGenerators(param));
                f.Dispose();
            }
        }

        private TreeNode Json2Tree(JObject obj)
        {
            // Если объекта нет, то возвращаем null.
            if (obj is null)
            {
                return null;
            }
            //Создаем родительскую вершину.
            TreeNode parent = new TreeNode();
            //Итерируемся по obj. Все токены явлются <key, value>
            foreach (var token in obj)
            {
                //change the display Content of the parent
                if (token.Key.ToString() == "name")
                {
                    parent.Text = token.Value.ToString();
                    parent.Name = parent.Text;
                }
                //create the child node
                TreeNode child = new TreeNode();
                child.Text = token.Key.ToString();
                //check if the value is of type obj recall the method
                if (token.Value.Type.ToString() == "Object")
                {                    
                    //create a new JObject using the the Token.value
                    JObject o = (JObject)token.Value;
                    //recall the method
                    child = Json2Tree(o);
                    child.Text = token.Key.ToString();
                    //add the child to the parentNode
                    parent.Nodes.Add(child);
                }
                //if type is of array
                else if (token.Value.Type.ToString() == "Array")
                {
                    int ix = -1;
                    //  child.Text = token.Key.ToString();
                    //loop though the array
                    foreach (var itm in token.Value)
                    {
                        //check if value is an Array of objects
                        if (itm.Type.ToString() == "Object")
                        {
                            TreeNode objTN = new TreeNode();
                            //child.Text = token.Key.ToString();
                            //call back the method
                            ix++;

                            JObject o = (JObject)itm;
                            objTN = Json2Tree(o);
                            objTN.Text = token.Key.ToString() + "[" + ix + "]";
                            child.Nodes.Add(objTN);
                            //parent.Nodes.Add(child);
                        }
                        //regular array string, int, etc
                        else if (itm.Type.ToString() == "Array")
                        {
                            ix++;
                            TreeNode dataArray = new TreeNode();
                            foreach (var data in itm)
                            {
                                dataArray.Text = token.Key.ToString() + "[" + ix + "]";
                                dataArray.Nodes.Add(data.ToString());
                            }
                            child.Nodes.Add(dataArray);
                        }

                        else
                        {
                            child.Nodes.Add(itm.ToString());
                        }
                    }
                    parent.Nodes.Add(child);
                }
                else
                {
                    child.Text = token.Key.ToString();
                    //change the value into N/A if value == null or an empty string 
                    if (token.Value.ToString() == "")
                        child.Nodes.Add("N/A");
                    else
                        child.Nodes.Add(token.Value.ToString());
                    parent.Nodes.Add(child);

                }
            }
            return parent;

        }

        private void UpdateCircuitsList()
        {
            IEnumerable<string> allfiles = Directory.EnumerateFiles(Settings.datasetPath, "*.json", SearchOption.AllDirectories);
            trvwCircuits.Nodes.Clear();
            foreach (string filename in allfiles)
            {
                string s = File.ReadAllText(filename);
                JObject obj = JObject.Parse(s);      
                TreeNode parent = Json2Tree(obj);
                int n = filename.LastIndexOf('.');
                TreeNode path = AddNode("path", filename.Substring(0, n == -1 ? filename.Length : n));
                path.Name = "path";
                parent.Nodes.Add(path);
                trvwCircuits.Nodes.Add(parent);

            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (isGeneratingRunning && !backgroundWorker1.IsBusy && !backWorkerStopWatch.IsBusy)
            {
                popup = new PopupNotifier();
                popup.TitleText = "Генерация";
                popup.ContentText = "Процесс генерации уже запущен. Дождитесь завершения процесса генерации.";
                popup.Popup();
                return;
            }           

            isGeneratingRunning = true;

            popup = new PopupNotifier();
            popup.TitleText = "Генерация";
            popup.ContentText = "Процесс генерации запущен.";
            popup.Popup();

            stopWatch = new Stopwatch();
            backgroundWorker1.RunWorkerAsync();            
        }

        private void datasetPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Выберите папку с датасетом.";
            fbd.ShowNewFolderButton = false;
            fbd.SelectedPath = Environment.CurrentDirectory;
            var res = fbd.ShowDialog();
            if (res == DialogResult.OK)
            {
                Settings.datasetPath = fbd.SelectedPath;
                UpdateCircuitsList();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (trvwCircuits.SelectedNode != null)
            {
                foreach (TreeNode node in trvwCircuits.Nodes)
                    if (Contains(node, trvwCircuits.SelectedNode))
                    {
                        int nd = trvwCircuits.Nodes.IndexOf(node);
                        int nd2 = trvwCircuits.Nodes[nd].Nodes.IndexOfKey("path");
                        CircuitForm f = new CircuitForm(trvwCircuits.Nodes[nd].Nodes[nd2].Nodes[0].Text);
                        DialogResult res = f.ShowDialog();
                        return;
                    }
            }            
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            backgroundWorker1.ReportProgress(0, "Генерация...");
            stopWatch.Start();
            backWorkerStopWatch.RunWorkerAsync();

            for (int i = 0; i < parameters.Count; i++)
            {
                generator = new DataBaseGenerator();
                generator.GenerateType(parameters[i], false);
                backgroundWorker1.ReportProgress((100 * i) / parameters.Count, "Генерация...");
            }

            backgroundWorker1.ReportProgress(100, "Выполнено!");
        }

        private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            toolStripProgressBar1.Value = e.ProgressPercentage;
            statusStrip1.Invoke((MethodInvoker)(() => toolStripStatusLabel1.Text = e.UserState as String));
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            stopWatch.Stop();
            generator = null;
            GC.Collect();
            toolStripStatusLabel1.Text = "Ожидание";
            UpdateCircuitsList();
            isGeneratingRunning = false;           

            popup = new PopupNotifier();
            popup.TitleText = "Генерация";
            popup.ContentText = "Процесс генерации закончен.";
            popup.Popup();
        }

        private void backWorkerStopWatch_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            TimeSpan ts = stopWatch.Elapsed;
            while (backgroundWorker1.IsBusy)
            {
                ts = stopWatch.Elapsed;
                backWorkerStopWatch.ReportProgress(0, String.Format("{0:00}.{1:00}:{2:00}:{3:00}",
                                                        ts.Days, ts.Hours, ts.Minutes, ts.Seconds));
                Thread.Sleep(500);
            }
        }
        private void backWorkerStopWatch_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            statusStrip1.Invoke((MethodInvoker)(() => lblStopWatch.Text = e.UserState as String));
        }
        private void backWorkerStopWatch_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            TimeSpan ts = stopWatch.Elapsed;
            statusStrip1.Invoke((MethodInvoker)(() => lblStopWatch.Text = String.Format("{0:00}.{1:00}:{2:00}:{3:00}",
                                                        ts.Days, ts.Hours, ts.Minutes, ts.Seconds)));
        }
        private void btnCancelGeneration_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy)
            {
                backgroundWorker1.CancelAsync();
                stopWatch.Stop();
                if (backWorkerStopWatch.IsBusy)
                    backWorkerStopWatch.CancelAsync();
                generator = null;
                GC.Collect();
                toolStripStatusLabel1.Text = "Ожидание";
                UpdateCircuitsList();
                isGeneratingRunning = false;

                popup = new PopupNotifier();
                popup.TitleText = "Генерация";
                popup.ContentText = "Процесс генерации отменен.";
                popup.Popup();
            }
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.UpdateCircuitsList();
        }

        private void генерацияCsvToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filePath = Settings.datasetPath + "\\" + Settings.csvdataset;
            string delimiter = ",";
            string[] head = new string[]{
                "numInputs", "numOutputs", "maxLevel",
                "const", "and", "nand", "or", "nor",
                "not", "xor", "xnor", "buf"};//, "verilog", "verilog_nangate"};

            string[] el_in = new string[]{ "const", "and", "nand", "or", "nor",
                "not", "xor", "xnor", "buf", "input"};
            string[] el_out = new string[]{"and", "nand", "or", "nor",
                "not", "xor", "xnor", "buf", "output"};
            foreach (string s1 in el_in)
                foreach (string s2 in el_out)
                    head = head.Append<string>(s1 + "-" + s2).ToArray();

            head = head.Append<string>("reliability_percent").ToArray();
            //head = head.Append<string>("sensitive_area_percent").ToArray();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Join(delimiter, head));

            IEnumerable<string> allfiles = Directory.EnumerateFiles(Settings.datasetPath, "*.json", SearchOption.AllDirectories);
            foreach (string filename in allfiles)
            {
                string s = File.ReadAllText(filename);
                JObject obj = JObject.Parse(s);
                Dictionary<string, string> dict= new Dictionary<string, string>();
                foreach (string sub in head)
                    dict.Add(sub, "0");
                
                foreach (var token in obj)
                {
                    if (head.Contains(token.Key.ToString()))
                        dict[token.Key.ToString()] = token.Value.ToString().Replace(",", ".");

                    if (token.Value.Type.ToString() == "Object")
                    {
                        JObject o = (JObject)token.Value;
                        foreach (var tok in (JObject)token.Value)
                        {
                            if (head.Contains(tok.Key.ToString()))
                            {
                                dict[tok.Key.ToString()] = tok.Value.ToString().Replace(",", ".");
                            }
                        }
                    }
                }

                //dict["verilog"] = string.Join("\n", File.ReadAllLines(filename.Replace(".json", ".v")));
                //dict["verilog_nangate"] = string.Join("\n", File.ReadAllLines(filename.Replace(".json", "_Nangate.v")));

                string[] ss = new string[head.Length];
                for (int i = 0; i < head.Length; i++)
                    ss[i] = dict[head[i]];

                sb.AppendLine(string.Join(delimiter, ss));
            }

            File.WriteAllText(filePath, sb.ToString());
        }

        private void добавитьСторонниеСхемыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = Settings.datasetPath;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                Console.WriteLine("Start parsing");
                IEnumerable<string> allfiles = Directory.EnumerateFiles(fbd.SelectedPath, "*.v", SearchOption.AllDirectories);
                foreach (string file in allfiles)
                {
                    if (!File.Exists(file.Replace(".v", ".json"))){
                        string dir = Path.GetDirectoryName(file);
                        FileInfo fileInfo = new FileInfo(file);
                        string name = fileInfo.Name;
                        string name2 = fileInfo.Name.Replace(".v", "_old.v");
                        File.Copy(Path.Combine(dir, name), Path.Combine(dir, name2), true);
                        Console.WriteLine(file);
                        Circuit circuit = AuxiliaryMethods.ParseVerilog(file);
                        circuit.generate(true);
                    }
                }
                Console.WriteLine("Complete");
            }
        }

        private void подсчетНадежностиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = Settings.datasetPath;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                Console.WriteLine("Start calc");
                IEnumerable<string> allfiles = Directory.EnumerateFiles(fbd.SelectedPath, "*.v", SearchOption.AllDirectories);
                foreach (string file in allfiles)
                {
                    if (File.Exists(file.Replace(".v", "_Nangate.v")))
                    {
                        string path = Path.GetDirectoryName(file);
                        FileInfo fileInfo = new FileInfo(file);
                        string circuitName = fileInfo.Name.Replace(".v", "");

                        {
                            string s = File.ReadAllText(path + "\\" + circuitName + ".json");
                            if (s.IndexOf("gates") != -1)
                                continue;
                        }

                            Dictionary<string, double> dict = new Dictionary<string, double>
                        {
                            {"gates", 0},
                            {"sensitivity_factor", 0},
                            {"sensitivity_factor_percent", 0},
                            {"sensitive_area", 0},
                            {"sensitive_area_percent", 0},
                        };
                        string curPath = Directory.GetCurrentDirectory();
                        Process cmd = new Process();
                        cmd.StartInfo.FileName = "cmd.exe";
                        cmd.StartInfo.RedirectStandardInput = true;
                        cmd.StartInfo.RedirectStandardOutput = true;
                        cmd.StartInfo.CreateNoWindow = true;
                        cmd.StartInfo.UseShellExecute = false;
                        cmd.StartInfo.WorkingDirectory = curPath;
                        cmd.Start();

                        cmd.StandardInput.WriteLine("cd " + Settings.pathNadezhda);
                        cmd.StandardInput.Write(Settings.nadezhda["python"] + " ");
                        cmd.StandardInput.Write(Settings.nadezhda["reliability"] + " ");
                        cmd.StandardInput.Write(path + "\\" + circuitName + "_Nangate.v ");
                        cmd.StandardInput.Write(Settings.nadezhda["liberty"] + " ");
                        cmd.StandardInput.Write(path + "\\report.txt");
                        cmd.StandardInput.WriteLine();                        

                        cmd.StandardInput.Flush();
                        cmd.StandardInput.Close();
                        cmd.WaitForExit();

                        if (File.Exists(path + "\\report.txt"))
                        {
                            string s = File.ReadAllText(path + "\\report.txt");
                            int start = 0;
                            start = s.IndexOf(": ");
                            int end = s.IndexOf('\n', start);
                            string sub = s.Substring(start + 2, end - start - 2);
                            sub = sub.Replace(".", ",");
                            dict["gates"] = Convert.ToDouble(AuxiliaryMethods.RemoveSpaces(sub));
                            start = end;

                            start = s.IndexOf(": ", start);
                            end = s.IndexOf(' ', start + 2);
                            sub = s.Substring(start + 2, end - start - 2);
                            sub = sub.Replace(".", ",");
                            dict["sensitivity_factor"] = Convert.ToDouble(AuxiliaryMethods.RemoveSpaces(sub));
                            start = end;

                            start = s.IndexOf(" ", start);
                            end = s.IndexOf('\n', start + 1);
                            sub = s.Substring(start + 2, end - start - 5);
                            sub = sub.Replace(".", ",");
                            dict["sensitivity_factor_percent"] = Convert.ToDouble(AuxiliaryMethods.RemoveSpaces(sub));
                            start = end;

                            start = s.IndexOf(": ", start);
                            end = s.IndexOf(' ', start + 2);
                            sub = s.Substring(start + 2, end - start - 2);
                            sub = sub.Replace(".", ",");
                            dict["sensitive_area"] = Convert.ToDouble(AuxiliaryMethods.RemoveSpaces(sub));
                            start = end;

                            start = s.IndexOf(" ", start);
                            end = s.IndexOf('\n', start + 1);
                            sub = s.Substring(start + 2, end - start - 5);
                            sub = sub.Replace(".", ",");
                            dict["sensitive_area_percent"] = Convert.ToDouble(AuxiliaryMethods.RemoveSpaces(sub));

                            File.Delete(path + "\\report.txt");
                        }
                        

                        if (File.Exists(path + "\\" + circuitName + ".json"))
                        {
                            string s = File.ReadAllText(path + "\\" + circuitName + ".json");
                            s = s.Remove(s.LastIndexOf('}'));
                            s += "	\"gates\": " + Convert.ToInt32(dict["gates"]).ToString() + ",\r\n";
                            s += "	\"sensitivity_factor\": " + dict["sensitivity_factor"].ToString().Replace(",", ".") + ",\r\n";
                            s += "	\"sensitivity_factor_percent\": " + dict["sensitivity_factor_percent"].ToString().Replace(",", ".") + ",\r\n";
                            s += "	\"sensitive_area\": " + dict["sensitive_area"].ToString().Replace(",", ".") + ",\r\n";
                            s += "	\"sensitive_area_percent\": " + dict["sensitive_area_percent"].ToString().Replace(",", ".") + ",\r\n}";

                            File.WriteAllText(path + "\\" + circuitName + ".json", s);
                        }
                    }
                }
                Console.WriteLine("Complete");                
            }
        }
    }
}
