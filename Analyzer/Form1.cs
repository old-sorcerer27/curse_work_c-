using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace SyntaxAnalyzer
{
    public struct ServiceWord
    {
        public int id;
        public string word;

        public ServiceWord(int _id, string _word)
        {
            id = _id;
            word = _word;
        }
    }

    public struct Separators
    {
        public int id;
        public string separator;

        public Separators(int _id, string _separator)
        {
            id = _id;
            separator = _separator;
        }
    }

    public struct Number
    {
        public int id;
        public string number;

        public Number(int _id, string _number)
        {
            id = _id;
            number = _number;
        }
    }

    public struct Identificator
    {
        public int id;
        public string identificator;

        public Identificator(int _id, string _identificator)
        {
            id = _id;
            identificator = _identificator;
        }
    }
    public partial class Form1 : Form
    {
        public string _programmText1;

        public List<Identificator> _identificators = new List<Identificator>();
        public List<Number> _numbers = new List<Number>();

        public List<ServiceWord> serviceWords = new List<ServiceWord>();
        public List<Separators> separators = new List<Separators>();

        public List<string> numbers = new List<string>();
        public List<string> identificators = new List<string>();

        public Dictionary<string, string> _initializedVariables = new Dictionary<string, string>();
        public Dictionary<string, string> _numberWithType = new Dictionary<string, string>();

        public List<string> operationsAssignments = new List<string>();
        public List<string> expression = new List<string>();


        public Form1()
        {
            InitializeComponent();
        }

       
        private void Form1_Load(object sender, EventArgs e)
        {
            System.Data.DataTable table_w = new System.Data.DataTable();
            table_w.Columns.Add("№", typeof(int));
            table_w.Columns.Add("Служебные слова", typeof(string));

            System.Data.DataTable table_s = new System.Data.DataTable();
            table_s.Columns.Add("№", typeof(int));
            table_s.Columns.Add("Разделители", typeof(string));

            var serviceWordList = DataTable.GetServiceWords();
            var separatorsList = DataTable.GetSeparators();

            for (int i = 0; i < serviceWordList.Count; i++)
			{
                serviceWords.Add(new ServiceWord(i, serviceWordList[i]));
                table_w.Rows.Add(serviceWords[i].id, serviceWords[i].word);
                
            }

			for (int i = 0; i < separatorsList.Count; i++)
			{
                separators.Add(new Separators(i, separatorsList[i]));
                table_s.Rows.Add(separators[i].id, separators[i].separator);
            }

            dataGridView1.DataSource = table_w;
            dataGridView1.Columns[0].Width = 30;
            dataGridView1.Columns[1].Width = 45;

            dataGridView2.DataSource = table_s;
            dataGridView2.Columns[0].Width = 30;
            dataGridView2.Columns[1].Width = 45;
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stream myStream;
            OpenFileDialog openFile = new OpenFileDialog();

            if(openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if((myStream = openFile.OpenFile()) != null)
                {
                    string strFileName = openFile.FileName;
                    string fileText = File.ReadAllText(strFileName);
                    richTextBox1.Text = fileText;
                    _programmText1 = Parser.StartParser(fileText);
                    richTextBox3.Text += "Файл " + strFileName + " открыт! \n";
                }
            }
        }

       

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            if(saveFile.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = saveFile.FileName;
            File.WriteAllText(filename, richTextBox1.Text);
            richTextBox3.Text += "Файл " + filename + " сохранён! \n";

        }

        public void Massage(string str)
        {
           
            richTextBox3.Text += (str + "\n");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        private void исходныйТекстПрограммыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        private void результатыАнализаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBoxResult.Clear();
        }

        private void окноУведомленийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox3.Clear();
        }

		private void лексическийToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (richTextBox1.Text != "")
			{

            }
		}

        public void CatchError (string errorStr)
		{
            richTextBox3.Text += $"{errorStr}\n";
        }

        public void SetTableNumbers(List<Number> numbers)
		{
            System.Data.DataTable table_k = new System.Data.DataTable();
            table_k.Columns.Add("№", typeof(int));
            table_k.Columns.Add("Константы", typeof(string));

            for (int i = 0; i < numbers.Count; i++)
            {
                table_k.Rows.Add(numbers[i].id, numbers[i].number);
            }
            dataGridView4.DataSource = table_k;
            dataGridView4.Columns[0].Width = 30;
            dataGridView4.Columns[1].Width = 85;
        }

        public void SetTableIdentificators(List<Identificator> identificator)
		{
            System.Data.DataTable table_id = new System.Data.DataTable();
            table_id.Columns.Add("№", typeof(int));
            table_id.Columns.Add("Идентификаторы", typeof(string));

            for (int i = 0; i < identificator.Count; i++)
            {
                table_id.Rows.Add(identificator[i].id, identificator[i].identificator);
            }

            dataGridView3.DataSource = table_id;
            dataGridView3.Columns[0].Width = 30;
            dataGridView3.Columns[1].Width = 85;
        }

        private void СинтаксическийToolStripMenuItem_Click(object sender, EventArgs e)
		{
            if (richTextBox1.Text != "")
            {

            }
        }

        private void семантическийToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text != "")
            {

            }
        }


        private void анализToolStripMenuItem_Click(object sender, EventArgs e)
		{
            if (richTextBox1.Text != "")
            {
                richTextBox3.Text += "Запущен лексический анализатор! \n";
                LexicalAnalyzer lexicalAnalyzer = new LexicalAnalyzer(this);
                richTextBoxResult.Text = lexicalAnalyzer.StartLexicalAnalyzer(_programmText1);
                richTextBox3.Text += "Лексический анализ завершен! \n";
                SetTableIdentificators(_identificators);
                SetTableNumbers(_numbers);


                richTextBox3.Text += "Запущен синтаксический анализатор! \n";
                SyntacticAnalyzer syntacticAnalyzer = new SyntacticAnalyzer(this);
                syntacticAnalyzer.CheckProgram(_programmText1);
                richTextBox3.Text += "Синтаксический анализ завершен! \n";


                richTextBox3.Text += "Запущен семантический анализатор! \n";
                SemanticAnalyzer semanticAnalyzer = new SemanticAnalyzer(this);
                semanticAnalyzer.StartSemanticAnalyzer();
                richTextBox3.Text += "Семантический анализ завершен! \n";
            }
        }





        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }


        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


        private void очисткаToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void dataGridView4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }



        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox5_TextChanged(object sender, EventArgs e)
        {

        }


        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void оРазработчикеToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        public void richTextBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBoxResult_TextChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click_1(object sender, EventArgs e)
        {

        }

        private void label9_Click_1(object sender, EventArgs e)
        {

        }

        private void label10_Click_1(object sender, EventArgs e)
        {

        }

        private void dataGridView7_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView8_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView5_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView6_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void всёToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox3.Clear();
            richTextBoxResult.Clear();
            richTextBox1.Clear();
            richTextBox1.Clear();
        }
    }
}
