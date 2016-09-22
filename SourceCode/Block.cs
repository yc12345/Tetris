using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Test2
{
    class Block
    {
        private static int type = -1;//当前方块类型
        private static int nextType = -1;//下一块类型
        private static int[,] v = new int[18, 11];//虚拟平台数组
        private static int[,] vn = new int[5, 5];//下一块小界面虚拟平台数组
        private Point [] p=new Point[4];//方块的四个组成单元的位置
        private Point[] pn = new Point[4];//Next区域的四个方块
        private bool gameover=false;//游戏结束判定标志
        private int state=0;//当前方块姿态
        private static bool[] isFull = new bool[17];//各行是否铺满

        public static int getVirtual(int i,int j)//虚拟数组get函数
        {
            return v[i,j];
        }
        public static int getVirtualN(int i,int j)//下一块虚拟数组get函数
        {
            return vn[i, j];
        }
        public bool getGameOver()//游戏结束判定获取函数
        {
            return gameover;
        }

        public static int getType()//获取当前块类型
        {
            return type;
        }
        public static int getNextType()//获取下一块类型
        {
            return nextType;
        }
        public static bool getIsFull(int i)//获取某行是否铺满
        {
            return isFull[i];
        }
        public Block()//构造函数
        {
            state = 0;
            Initial_IsFull();
        }
        public void Initial_IsFull()//行铺满判定数组初始化
        {
            for (int i = 1; i <= 16; i++)
                isFull[i] = false;
        }
        public static void Virtual_Process()//虚拟数组的预处理
        {
            //大界面预处理：边界-1，空0，移动块1，固定块2
            for (int i = 1; i <= 16; i++)
                for (int j = 1; j <= 9; j++)
                    v[i, j] = 0;
            for (int i = 0; i <= 10; i++)
                v[0, i] = v[17, i] = -1;
            for (int i = 1; i <= 16; i++)
                v[i, 0] = v[i, 10] = -1;
            //小界面预处理
            for (int i = 1; i <= 4; i++)
                for (int j = 1; j <= 4; j++)
                    vn[i, j] = 0;
        }

        public static void Type_Rand()//随机生成当前方块种类
        {
            Random random = new Random();
            type = random.Next(1, 8);
        }

        public static void NextType_Rand()//随机生成下一块种类
        {
            Random random = new Random();
            nextType = random.Next(1, 8);
        }
        public static void AssignType()//将下一块值赋给当前方块
        {
            type = nextType;
        }
        public Point[] Initial_Point(int t)//初始化Point数组，存放大界面中各种块初始姿态下的坐标
        {
            Point[] p = new Point[4];
            switch(t)
            {
                case 1:
                    p[0] = new Point(1, 5);
                    p[1] = new Point(2, 4);
                    p[2] = new Point(2, 5);
                    p[3] = new Point(2, 6);
                    break;
                case 2:
                    p[0] = new Point(1, 5);
                    p[1] = new Point(2, 5);
                    p[2] = new Point(3, 5);
                    p[3] = new Point(4, 5);
                    break;
                case 3:
                    p[0] = new Point(1, 5);
                    p[1] = new Point(1, 6);
                    p[2] = new Point(2, 5);
                    p[3] = new Point(2, 6);
                    break;
                case 4:
                    p[0] = new Point(1, 5);
                    p[1] = new Point(2, 5);
                    p[2] = new Point(2, 6);
                    p[3] = new Point(3, 6);
                    break;
                case 5:
                    p[0] = new Point(1, 5);
                    p[1] = new Point(2, 5);
                    p[2] = new Point(2, 4);
                    p[3] = new Point(3, 4);
                    break;
                case 6:
                    p[0] = new Point(1, 5);
                    p[1] = new Point(2, 5);
                    p[2] = new Point(3, 5);
                    p[3] = new Point(3, 6);
                    break;
                case 7:
                    p[0] = new Point(1, 5);
                    p[1] = new Point(2, 5);
                    p[2] = new Point(3, 5);
                    p[3] = new Point(3, 4);
                    break;
            }
            return p;
        }
        public Point[] Initial_Next(int nt)//初始化PointN数组，存放小界面中各种块初始姿态下的坐标
        {
            Point[] p=new Point[4];
            switch(nt)
            {
                case 1:
                    p[0] = new Point(2, 2);
                    p[1] = new Point(3, 1);
                    p[2] = new Point(3, 2);
                    p[3] = new Point(3, 3);
                    break;
                case 2:
                    p[0] = new Point(1, 2);
                    p[1] = new Point(2, 2);
                    p[2] = new Point(3, 2);
                    p[3] = new Point(4, 2);
                    break;
                case 3:
                    p[0] = new Point(2, 2);
                    p[1] = new Point(2, 3);
                    p[2] = new Point(3, 2);
                    p[3] = new Point(3, 3);
                    break;
                case 4:
                    p[0] = new Point(1, 2);
                    p[1] = new Point(2, 2);
                    p[2] = new Point(2, 3);
                    p[3] = new Point(3, 3);
                    break;
                case 5:
                    p[0] = new Point(1, 3);
                    p[1] = new Point(2, 3);
                    p[2] = new Point(2, 2);
                    p[3] = new Point(3, 2);
                    break;
                case 6:
                    p[0] = new Point(1, 2);
                    p[1] = new Point(2, 2);
                    p[2] = new Point(3, 2);
                    p[3] = new Point(3, 3);
                    break;
                case 7:
                    p[0] = new Point(1, 3);
                    p[1] = new Point(2, 3);
                    p[2] = new Point(3, 3);
                    p[3] = new Point(3, 2);
                    break;
            }
            return p;
        }
        public void Initial_Point_Main()//大界面Point数组初始化
        {
            p = Initial_Point(type);
        }
        public void Initial_Point_Next()//小界面PointN数组初始化
        {
            pn = Initial_Next(nextType);
        }
        public void Initial_Virtual_By_Point()//根据Point数组初始化虚拟数组的相应位置
        {
            //大界面初始化，初始化失败则游戏结束
            bool flag = true;//判定初始化是否成功
            for (int i = 0; i < 4; i++)
                if (v[p[i].X, p[i].Y] != 0)
                    flag = false;
            if (flag)
            {
                for (int i = 0; i < 4; i++)
                    v[p[i].X, p[i].Y] = 1;
            }
            else
            {
                for (int i = 0; i < 4; i++)
                    if (v[p[i].X, p[i].Y] == 0)
                        v[p[i].X, p[i].Y] = 1;
                    gameover = true;
            }
            //小界面一定能成功初始化，故不用判定
            for (int i = 0; i < 4; i++)
                vn[pn[i].X, pn[i].Y] = 1;
        }

        public void translate(int LeftOrRight)//左右平移，通过改变虚拟数组相应状态实现块的平移
        {
            if(LeftOrRight==1)//左移
            {
                bool flag=true;
                for (int i = 0; i < 4; i++)
                    if (v[p[i].X , p[i].Y - 1] != 0 && v[p[i].X , p[i].Y - 1] != 1)
                        flag = false;
                if(flag)
                {
                    //清零
                    for(int i=0;i<4;i++)
                        v[p[i].X, p[i].Y] = 0;
                    //设新值
                    for (int i = 0; i < 4; i++)
                        v[p[i].X , p[i].Y - 1] = 1;
                    //重置点位置
                    for (int i = 0; i < 4; i++)
                        p[i].Y--;
                }
            }
            if(LeftOrRight==2)
            {
                bool flag=true;
                for (int i = 0; i < 4; i++)
                    if (v[p[i].X , p[i].Y + 1] != 0 && v[p[i].X , p[i].Y + 1] != 1)
                        flag = false;
                if (flag)
                {
                    //清零
                    for (int i = 0; i < 4; i++)
                        v[p[i].X, p[i].Y] = 0;
                    //设新值
                    for (int i = 0; i < 4; i++)
                        v[p[i].X , p[i].Y + 1] = 1;
                    //重置点位置
                    for (int i = 0; i < 4; i++)
                        p[i].Y++;
                }
            }
        }
        public bool Check()//下落状态检查
        {
            bool flag = true;
            for (int i = 0; i < 4; i++)
                if (v[p[i].X + 1, p[i].Y] == -1 || v[p[i].X + 1, p[i].Y] == 2)
                    flag = false;
                return flag;
        }
        public void Fall()//下落
        {
            for (int i = 0; i < 4; i++)
                v[p[i].X, p[i].Y] = 0;
            for (int i = 0; i < 4; i++)
                v[p[i].X + 1, p[i].Y] = 1;
            for (int i = 0; i < 4; i++)
                p[i].X++;
        }
        public void Fixed()//将移动块固定
        {
            for (int i = 0; i < 4; i++)
                v[p[i].X, p[i].Y] = 2;
        }

        public void rotate()//块旋转
        {
            if(Rotate_Check(type,state))
            {
                Set_Zero();
                Point[] next = NextState(type, state);
                for (int i = 0; i < 4; i++)
                    v[next[i].X, next[i].Y] = 1;
                for (int i = 0; i < 4; i++)
                    p[i] = next[i];
                Set_State(type);
            }
        }

        public void Set_Zero()//虚拟数组移动块相应位置清空
        {
            for (int i = 0; i < 4; i++)
                v[p[i].X, p[i].Y] = 0;
        }

        public void Set_State(int t)//更新当前状态到下一状态
        {
            switch(t)
            {
                case 1:
                    state = (state + 1) % 4;
                    break;
                case 2:
                    state = (state + 1) % 2;
                    break;
                case 3:
                    break;
                case 4:
                    state = (state + 1) % 2;
                    break;
                case 5:
                    state = (state + 1) % 2;
                    break;
                case 6:
                    state = (state + 1) % 4;
                    break;
                case 7:
                    state = (state + 1) % 4;
                    break;
            }
        }
        public bool Rotate_Check(int t,int st)//检查能否成功旋转
        {
            Point[] next = NextState(t, st);
            bool flag = true;
            for (int i = 0; i < 4; i++)
                if (v[next[i].X, next[i].Y] != 0 && v[next[i].X, next[i].Y] != 1)
                    flag = false;
            return flag;
        }

        public Point [] NextState(int t,int st)//当前方块旋转到下一个状态的各点位置
        {
            Point[] point = new Point[4];
            switch (t)
            {
                case 1:
                    switch (st)
                    {
                        case 0:
                            point = new Point[] { p[0], new Point(p[1].X + 1, p[1].Y + 1), p[2], p[3] };
                            break;
                        case 1:
                            point = new Point[] { new Point(p[0].X + 1, p[0].Y - 1), p[1], p[2], p[3] };
                            break;
                        case 2:
                            point = new Point[] { p[0], p[1], p[2], new Point(p[3].X - 1, p[3].Y - 1) };
                            break;
                        case 3:
                            point = new Point[] { new Point(p[0].X-1,p[0].Y+1), new Point(p[1].X - 1, p[1].Y -1 ), p[2], new Point(p[3].X+1,p[3].Y+1) };
                            break;
                    }
                    break;
                case 2:
                    switch (st)
                    {
                        case 0:
                            point = new Point[] { new Point(p[0].X + 2, p[0].Y + 2), new Point(p[1].X + 1, p[1].Y + 1), p[2], new Point(p[3].X - 1, p[3].Y - 1) };
                            break;
                        case 1:
                            point = new Point[] { new Point(p[0].X - 2, p[0].Y - 2), new Point(p[1].X - 1, p[1].Y - 1), p[2], new Point(p[3].X + 1, p[3].Y + 1) };
                            break;
                    }
                    break;
                case 3:
                    point = new Point[] { p[0], p[1], p[2], p[3] };
                    break;
                case 4:
                    switch (st)
                    {
                        case 0:
                            point = new Point[] { new Point(p[0].X + 1, p[0].Y + 1), p[1], new Point(p[2].X + 1, p[2].Y - 1), new Point(p[3].X, p[3].Y - 2) };
                            break;
                        case 1:
                            point = new Point[] { new Point(p[0].X - 1, p[0].Y - 1), p[1], new Point(p[2].X - 1, p[2].Y + 1), new Point(p[3].X, p[3].Y + 2) };
                            break;
                    }
                    break;
                case 5:
                    switch (st)
                    {
                        case 0:
                            point = new Point[] { new Point(p[0].X + 1, p[0].Y - 1), p[1], new Point(p[2].X + 1, p[2].Y + 1), new Point(p[3].X, p[3].Y + 2) };
                            break;
                        case 1:
                            point = new Point[] { new Point(p[0].X - 1, p[0].Y + 1), p[1], new Point(p[2].X - 1, p[2].Y - 1), new Point(p[3].X, p[3].Y - 2) };
                            break;
                    }
                    break;
                case 6:
                    switch (st)
                    {
                        case 0:
                            point = new Point[] { new Point(p[0].X + 1, p[0].Y + 1), p[1], new Point(p[2].X - 1, p[2].Y - 1), new Point(p[3].X, p[3].Y - 2) };
                            break;
                        case 1:
                            point = new Point[] { new Point(p[0].X + 1, p[0].Y - 1), p[1], new Point(p[2].X - 1, p[2].Y + 1), new Point(p[3].X - 2, p[3].Y) };
                            break;
                        case 2:
                            point = new Point[] { new Point(p[0].X - 1, p[0].Y - 1), p[1], new Point(p[2].X + 1, p[2].Y + 1), new Point(p[3].X, p[3].Y + 2) };
                            break;
                        case 3:
                            point = new Point[] { new Point(p[0].X - 1, p[0].Y + 1), p[1], new Point(p[2].X + 1, p[2].Y - 1), new Point(p[3].X + 2, p[3].Y) };
                            break;
                    }
                    break;
                case 7:
                    switch (st)
                    {
                        case 0:
                            point = new Point[] { new Point(p[0].X + 1, p[0].Y + 1), p[1], new Point(p[2].X - 1, p[2].Y - 1), new Point(p[3].X - 2, p[3].Y) };
                            break;
                        case 1:
                            point = new Point[] { new Point(p[0].X + 1, p[0].Y - 1), p[1], new Point(p[2].X - 1, p[2].Y + 1), new Point(p[3].X, p[3].Y + 2) };
                            break;
                        case 2:
                            point = new Point[] { new Point(p[0].X - 1, p[0].Y - 1), p[1], new Point(p[2].X + 1, p[2].Y + 1), new Point(p[3].X + 2, p[3].Y) };
                            break;
                        case 3:
                            point = new Point[] { new Point(p[0].X - 1, p[0].Y + 1), p[1], new Point(p[2].X + 1, p[2].Y - 1), new Point(p[3].X, p[3].Y - 2) };
                            break;
                    }
                    break;
            }
            return point;
        }
        public void Check_ClearUp()//每当当前方块固定，检查每行是否满足消除条件，并更新isFull数组的状态
        {
            for(int i=1;i<=16;i++)
            {
                bool flag = true;
                for(int j=1;j<=9;j++)
                {
                    if (v[i, j] == 0)
                        flag = false;
                }
                if(flag)
                {
                    isFull[i] = true;
                }
            }
        }
        public int ClearUp()//根据isFull数组的状态逐行检查并消除，根据一次性消除的行数返回本次得分
        {
            int addScore = 0;
            int addTime = 0;
            int i = 16;
            while(i>=1)
            {
                if(isFull[i])
                {
                    for(int j=i-1;j>=1;j--)
                    {
                        for(int k=1;k<=9;k++)
                        {
                            v[j + 1, k] = v[j,k];
                        }
                        isFull[j + 1] = isFull[j];
                    }
                    for (int k = 1; k <= 9; k++)
                        v[1, k] = 0;
                    isFull[1] = false;
                    i = 17;
                    addTime++;
                }
                i--;
            }
            switch(addTime)
            {
                case 0:
                    addScore = 0;
                    break;
                case 1:
                    addScore = 10;
                    break;
                case 2:
                    addScore = 30;
                    break;
                case 3:
                    addScore = 50;
                    break;
                case 4:
                    addScore = 80;
                    break;
            }
            return addScore;
        }
        public static void Reset()//大界面虚拟数组全部归零
        {
            for (int i = 1; i <= 16; i++)
                for (int j = 1; j <= 9; j++)
                    v[i, j] = 0;
        }
        public static void Next_Reset()//小界面虚拟数组全部归零
        {
            for (int i = 1; i <= 4; i++)
                for (int j = 1; j <= 4; j++)
                    vn[i, j] = 0;
        }
    }
}
