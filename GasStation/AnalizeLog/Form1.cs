using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnalizeLog
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Открытие файла лога
            var path = "";

            var ofDialog = new OpenFileDialog();

            if (ofDialog.ShowDialog()==DialogResult.OK)
            {
                path = ofDialog.FileName;
            }
            //Разбор лога в другом классе
            var file = ReaderLog.ReadChamberSection(path);

            var tv = new TeorVer(file);

            var listDt = tv.Dts;
            //Вычисление необходимых значений
            var maxDt = listDt.Max();
            //Вывод результата

            textBoxMaxDt.Text = $"{maxDt}";
        }
    }
}
