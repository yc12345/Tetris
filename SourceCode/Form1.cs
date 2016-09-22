using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Test2
{
    public partial class Form1 : Form
    {
        private static Panel[] p= new Panel[145];//游戏主界面的144个面板
        private static Panel[] pn = new Panel[17];//Next界面的16个面板
        private static int count;//统计面板数量
        private static int countn;//统计Next界面的面板数量
        private static Block b;//每一个进入方块
        private static Thread t;//游戏的第二线程
        private static bool undergame = false;//是否处于游戏状态中
        private static int DelayTime;//延时长度
        private static int totalScore;//总得分

        public Form1()//构造函数
        {
            InitializeComponent();
            Panel_Process();//预处理面板数组
            //添加键盘事件处理
            this.KeyUp += new KeyEventHandler(this.Form_KeyUp);
            this.KeyDown += new KeyEventHandler(this.Form_KeyDown);
        }

        private void Panel_Process()//获取布局控件中的面板控件
        {
            count = (tableLayoutPanel1.RowCount) * (tableLayoutPanel1.ColumnCount);
            for (int i = 1; i <= count; i++)
                p[i] = (Panel)tableLayoutPanel1.Controls[i - 1];//主界面Panel
            countn = (tableLayoutPanel2.RowCount) * (tableLayoutPanel2.ColumnCount);
            for (int i = 1; i <= countn; i++)
                pn[i] = (Panel)tableLayoutPanel2.Controls[i - 1];//Next界面Panel
        }

        public static void delay(int z)//物理延时函数
        {
            for (int x = z; x > 0; x--)
                for (int y = 120; y > 0; y--) ;
        }

        public void Form1_Paint(object sender, PaintEventArgs e)//窗体绘制函数
        {
            Clear();//先将所有面板涂成灰色
            for(int i=1;i<=16;i++)
                for(int j=1;j<=9;j++)
                {
                    if (Block.getVirtual(i, j) == 1 || Block.getVirtual(i, j) == 2)
                        Paint_Panel(p[(i - 1) * 9 + j], Brushes.Blue);
                }//将有块的地方涂成蓝色
            for(int i=1;i<=4;i++)
                for(int j=1;j<=4;j++)
                {
                    if (Block.getVirtualN(i, j) == 1)
                        Paint_Panel(pn[(i - 1) * 4 + j], Brushes.Blue);
                }//将有块的地方涂成蓝色
        }

        public void button1_Click(object sender, EventArgs e)//start按钮
        {
            totalScore = 0;//总分归零
            textBox1.Text = totalScore.ToString();
            DelayTime = 1000000;//设置初始延时
            if (undergame == false||(t!=null&&t.ThreadState==ThreadState.Suspended))//线程为空表示第一次进入，先成为挂起状态表示重新开始
            {
                Clear();//将所有面板涂成灰色
                undergame = true;//进入游戏状态
                Block.Virtual_Process();//初始化虚拟数组
                t = new Thread(new ParameterizedThreadStart(Run));//创建新线程，该线程负责生成块及控制块的下落，主函数为Run
                t.Start(sender);//第二线程开启
            }
        }

        public void Run(object sender)//第二线程主函数
        {
            Clear();//将所有面板涂成灰色
            //绘制函数的必要参数
            PaintEventArgs pea = new PaintEventArgs(this.CreateGraphics(), new Rectangle(new Point(0, 0), new Size(329, 401)));
            Block.Type_Rand();//随机生成当前块状态
            while (true)//进入游戏的主循环
            {
                //创建一个新的进入块并进行相应初始化工作
                b = new Block();
                Block.NextType_Rand();//随机生成下一块状态
                b.Initial_Point_Main();//初始化主界面坐标
                b.Initial_Point_Next();//初始化Next界面坐标
                b.Initial_Virtual_By_Point();//根据坐标初始化虚拟数组
                if(b.getGameOver())//游戏结束
                {
                    undergame = false;//退出游戏状态
                    MessageBox.Show("Game Over");
                    t.Abort();//第二线程终止
                }
                Form1_Paint(sender, pea);//开始先绘制一次
                delay(DelayTime);//延时
                while (b.Check())//检查下落状态并下落
                {
                    b.Fall();
                    Form1_Paint(sender, pea);//下落过程每下落一格绘制一次
                    delay(DelayTime);//下落时间间隔
                }
                b.Fixed();//下落到底端固定不动
                b.Check_ClearUp();//检查并更新各行铺满状态
                int add = b.ClearUp();//根据各行铺满状态更新虚拟数组并计算该次得分
                totalScore += add;//将该次得分加到总分
                textBox1.Text = totalScore.ToString();
                Block.AssignType();//将下一块种类赋给当前块种类
                Block.Next_Reset();//Next界面清空，等待绘制
            }
        }

        private void Form_KeyUp(object sender,KeyEventArgs e)//键盘事件处理
        {
            switch(e.KeyData)
            {
                case Keys.Left://左移
                    if (b != null)
                        b.translate(1);
                    break;
                case Keys.Right://右移
                    if (b != null)
                        b.translate(2);
                    break;
                case Keys.Down://抬起时恢复正常速度
                    DelayTime = 1000000;
                    break;
                case Keys.Up://旋转
                    if (b != null)
                        b.rotate();
                    break;
            }
            //每次键盘事件绘制一次
            PaintEventArgs pea = new PaintEventArgs(this.CreateGraphics(), new Rectangle(new Point(0, 0), new Size(329, 401)));
            Form1_Paint(sender, pea);
        }

        private void Form_KeyDown(object sender,KeyEventArgs e)
        {
            switch(e.KeyData)
            {
                case Keys.Down://按下时加快下落速度
                    DelayTime = 100000;
                    break;
            }
        }

        protected override bool ProcessDialogKey(Keys keyData)//覆盖系统的键盘处理事件
        {
            if ( keyData == Keys.Down||keyData == Keys.Up||keyData == Keys.Left||keyData == Keys.Right)
                return false;
            else
                return base.ProcessDialogKey(keyData);
        }

        private void Paint_Panel(Panel p, Brush b)//绘制一个Panel
        {
            Graphics g = p.CreateGraphics();
            Rectangle rect = new Rectangle(new Point(0, 0), new Size(45, 45));
            PaintEventArgs pea = new PaintEventArgs(g, rect);
            Paint_Panel_Args(pea, b);
        }

        private void Paint_Panel_Args(PaintEventArgs e, Brush b)//绘制Panel的中间函数
        {
            Graphics g = e.Graphics;
            g.FillRectangle(b, e.ClipRectangle);
            g.Flush();
        }

        private void Clear()//将所有panel涂成灰色
        {
            for (int i = 1; i <= count; i++)
                Paint_Panel(p[i], Brushes.Gray);
            for (int i = 1; i <= countn; i++)
                Paint_Panel(pn[i], Brushes.Gray);
        }

        private void button2_Click(object sender, EventArgs e)//退出按钮
        {
            if (t != null&&t.ThreadState==ThreadState.Running)
                t.Suspend();//退出时挂起第二线程，后面是继续还是终止依据下一步指令
            Block.Reset();//主界面清空
            Block.Next_Reset();//Next界面清空
            Form2 f = new Form2();//返回欢迎界面
            f.Show();
            this.Visible = false;//本界面不再可见
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;//跨线程调用控件
        }
    }
}
