using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test2
{
    public partial class Form2 : Form
    {
        public Form2()//构造函数
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)//开始按钮
        {
            Form1 f = new Form1();//构造游戏界面
            f.Show();//显示游戏界面
            this.Visible = false;//本界面不再可见
        }

        private void button2_Click(object sender, EventArgs e)//退出按钮
        {
            System.Environment.Exit(0);
        }

        private void button3_Click(object sender, EventArgs e)//弹出帮助界面
        {
            Form3 f = new Form3();
            f.Show();
        }
    }
}
