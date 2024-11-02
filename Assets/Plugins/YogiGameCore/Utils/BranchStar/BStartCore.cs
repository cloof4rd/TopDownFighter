using System;
using System.Collections.Generic;


namespace YogiGameCore.Utils
{
    /// <summary>
    /// B星寻路算法工具类
    /// </summary>
    public class BStartCore
    {
        #region Property

        public static BStartCore GetInstance()
        {
            if (m_Instance == null)
            {
                m_Instance = new BStartCore();
            }

            return m_Instance;
        }

        private static BStartCore m_Instance;

        /// <summary>
        /// 所有地图节点
        /// </summary>
        public Node[,] nodes;

        /// <summary>
        /// 所有的GridPos
        /// </summary>
        private static List<List<GridPos>> AllGridPos;

        /// <summary>
        /// 地图X大小
        /// </summary>
        public UInt32 RowCount;

        /// <summary>
        /// 地图Y大小
        /// </summary>
        public UInt32 ColCount;

        /// <summary>
        /// 最大计算次数
        /// </summary>
        private UInt32 CalcMaxCount = 100;

        /// <summary>
        /// 计算队列, 保存了计算的点, 可用于排错
        /// </summary>
        private Queue<GridPos> CheckQueue = new Queue<GridPos>();

        #endregion

        int[] SearchSize = new int[] {3, 5, 7, 9, 11, 13, 15};

        private BStartCore()
        {
        }

        #region PublicMethod


        public void Init(UInt32 p_SizeX, UInt32 p_SizeY)
        {
            RowCount = p_SizeX;
            ColCount = p_SizeY;

            nodes = new Node[RowCount, ColCount];

            AllGridPos = new List<List<GridPos>>((int) p_SizeX);
            for (int i = 0; i < AllGridPos.Capacity; i++)
            {
                AllGridPos.Add(new List<GridPos>((int) p_SizeY));
            }
            
            for (int i = 0; i < AllGridPos.Capacity; i++)
            {
                // AllGridPos[i] = new List<GridPos>((int) p_SizeY);
                    
                for (int j = 0; j < AllGridPos[i].Capacity; j++)
                {
                    AllGridPos[i].Add(new GridPos(i, j));
                }
            }

            for (UInt32 i = 0; i < RowCount; i++)
            {
                for (UInt32 j = 0; j < ColCount; j++)
                {
                    nodes[i, j] = new Node(i, j, false);
                    SetBlock(i, j);
                }
            }
        }

        public bool IsBlock(UInt32 x, UInt32 y)
        {
            if (x > nodes.GetLength(0) || y > nodes.GetLength(1))
            {
                return true;
            }

            return nodes[x, y].IsBlock;
        }

        /// <summary>
        /// 判断这个格子内是否超出了1个(用于检测AId当前是否在禁区)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool IsOutOfBounds(UInt32 x, UInt32 y)
        {
            if (x >= nodes.GetLength(0) 
                || y >= nodes.GetLength(1))
            {
                return true;
            }
            
            return nodes[x, y].ChildCount > 1;
        }

        public void SetBlock(uint x, uint y)
        {

            nodes[x, y].AddChild();
        }

        public void SetRoute(UInt32 x, UInt32 y)
        {
            nodes[x, y].RemoveChild();
        }


        /// <summary>
        /// 获得距离这个点最近的可以走的点
        /// </summary>
        /// <returns></returns>
        public bool GetNearestPos(UInt32 x, UInt32 y, out GridPos nearestPos)
        {
            foreach (int coreSize in this.SearchSize)
            {
                // [-1 , 1]   Or  [-2 , 2 ] ...
                for (int iX = -(coreSize - 1) / 2; iX <= (coreSize - 1) / 2; iX++)
                {
                    for (int iY = -(coreSize - 1) / 2; iY <= (coreSize - 1) / 2; iY++)
                    {
                        if (!this.IsBlock((UInt32) (x + iX), (UInt32) (y + iY)))
                        {
                            nearestPos =
                                AllGridPos[(Int32) (x + iX)]
                                    [(Int32) (y + iY)]; //new GridPos((Int32)(x + iX), (Int32)(y + iY));
                            return true;
                        }
                    }
                }
            }

            nearestPos = default;
            return false;
        }

        /// <summary>
        /// 位移
        /// </summary>
        /// <param name="oldPos"></param>
        /// <param name="newPos"></param>
        public void ChangePos(UInt32 oldPosX, UInt32 oldPosY, UInt32 newPosX, UInt32 newPosY)
        {
            nodes[oldPosX, oldPosY].RemoveChild();
            nodes[newPosX, newPosY].AddChild();
        }

        /// <summary>
        /// 寻路
        /// </summary>
        public Queue<Route> FindRoute(GridPos origon, GridPos target, bool isNeedSmooth = true)
        {
            CheckQueue.Clear();
            Queue<Route> resultReal = new Queue<Route>();
            List<Route> result = CalcNode(origon, target);
            if (result.Count == 0)
            {
                result = CalcNode(target, origon);
                for (int i = result.Count - 1; i >= 0; i--)
                {
                    resultReal.Enqueue(result[i]);
                }
            }
            else
            {
                for (int i = 0; i < result.Count; i++)
                {
                    resultReal.Enqueue(result[i]);
                }
            }

            if (isNeedSmooth)
            {
                // 光滑路径
                if (resultReal.Count > 0)
                {
                    resultReal = SmoothRoute(resultReal);
                }
            }

            return resultReal;
        }

        #endregion

        #region Private

        /// <summary>
        /// 计算两个目标点的路径
        /// </summary>
        /// <param name="current"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        private List<Route> CalcNode(GridPos current, GridPos target)
        {
            List<Route> result = new List<Route>();
            List<FindingNode> findingNodes = new List<FindingNode>();
            List<GridPos> blackList = new List<GridPos>();

            FindingNode freeNode = new FindingNode(target, current, findingNodes, blackList, nodes, CheckQueue);

            for (int i = 0; i < CalcMaxCount; i++)
            {
                for (int j = findingNodes.Count - 1; j >= 0; j--)
                {
                    if (findingNodes[j].Find())
                    {
                        result = findingNodes[j].myRoute;
                        findingNodes.Clear();
                        blackList.Clear();
                        return result;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 光滑路径(移除1/3的路径点)
        /// </summary>
        /// <param name="p_Route"></param>
        /// <returns></returns>
        private Queue<Route> SmoothRoute(Queue<Route> p_Route)
        {
            Queue<BStartCore.Route> result = new Queue<BStartCore.Route>();
            List<BStartCore.Route> g = new List<BStartCore.Route>();


            BStartCore.Route p1 = p_Route.Dequeue();
            BStartCore.Route p2;
            BStartCore.Route p3;

            while (p_Route.Count > 0)
            {
                if (p_Route.Count > 0)
                {
                    p2 = p_Route.Dequeue();
                }
                else
                {
                    result.Enqueue(p1);
                    continue;
                }

                if (p_Route.Count > 0)
                {
                    p3 = p_Route.Dequeue();
                    // p1 + p2 + p3
                    // 如果x都相等, 那么就无法优化 如果三个点的x都相等, 那么就说明他们在同一个方向
                    if (p1.Pos.x == p2.Pos.x && p2.Pos.x == p3.Pos.x ||
                        p1.Pos.y == p2.Pos.y && p2.Pos.y == p3.Pos.y)
                    {
                        result.Enqueue(p1);
                        result.Enqueue(p3);
                    }
                    else
                    {
                        result.Enqueue(p1);
                        result.Enqueue(p3);
                    }

                    p1 = p3;
                }
                else
                {
                    result.Enqueue(p1);
                    result.Enqueue(p2);
                }
            }

            return result;
        }

        #endregion

        /// <summary>
        /// BranchStart寻路,自由节点,用于寻路
        /// </summary>
        class FindingNode
        {
            /// <summary>
            /// 目标节点位置
            /// </summary>
            GridPos m_targetPos;

            /// <summary>
            /// 当前节点位置
            /// </summary>
            GridPos m_currentPos;

            /// <summary>
            /// 这个节点经过的路径点
            /// </summary>
            public List<Route> myRoute;

            /// <summary>
            /// 所有的自由节点
            /// </summary>
            List<FindingNode> findingNodes;

            /// <summary>
            /// 黑名单, 该位置的节点没有通路
            /// </summary>
            List<GridPos> blackList;

            /// <summary>
            /// 寻找方向 1为向右 -1为向左 10为向上 -10为向下
            /// </summary>
            Int32 findDir;

            /// <summary>
            /// 所有地图节点信息
            /// </summary>
            Node[,] mapNodes;

            /// <summary>
            /// 是否寻找到了道路
            /// </summary>
            bool isFinded = false;

            /// <summary>
            /// 缓存, 用于存取自由节点探查路径
            /// </summary>
            Queue<GridPos> cache;

            public FindingNode()
            {
            }

            public FindingNode(GridPos targetPos, GridPos currentPos, List<FindingNode> findingNodes,
                List<GridPos> blackList, Node[,] mapNodes, Queue<GridPos> cache)
            {
                myRoute = new List<Route>();
                this.m_targetPos = targetPos;
                this.m_currentPos = currentPos;
                this.findingNodes = findingNodes;
                this.findingNodes.Add(this);
                this.blackList = blackList;
                this.mapNodes = mapNodes;
                this.cache = cache;
                PathCorrect();
            }

            /// <summary>
            /// 寻找目标道路
            /// </summary>
            /// <returns>是否寻找到了目标节点</returns>
            public bool Find()
            {
                if (isFinded)
                {
                    return true;
                }

                switch (findDir)
                {
                    case 1: //右
                        LookRight();
                        break;
                    case -1: //左
                        LookLeft();
                        break;
                    case 10: //上
                        LookUp();
                        break;
                    case -10: // 下
                        LookDown();
                        break;
                    default:
                        break;
                }

                return false;
            }

            /// <summary>
            /// 路径矫正 矫正当前节点的寻找方向
            /// </summary>
            void PathCorrect()
            {
                // 判断到底是偏差大还是Y偏差大 来决定偏向x还是y
                Int32 a = Math.Abs((m_targetPos.x - m_currentPos.x));
                Int32 b = Math.Abs((m_targetPos.y - m_currentPos.y));
                if (a > b)
                {
                    PathCorrectX();
                }
                else
                {
                    PathCorrectY();
                }
            }

            /// <summary>
            /// 优先选择在x方向
            /// </summary>
            void PathCorrectX()
            {
                // 目标在当前右边
                if (m_targetPos.x > m_currentPos.x)
                {
                    if (PeekRight()) // 右边堵住了
                    {
                        BranchVertical();
                    }
                    else
                    {
                        findDir = 1;
                    }
                }
                // 目标在当前左边
                else if (m_targetPos.x < m_currentPos.x)
                {
                    //左边堵住了
                    if (PeekLeft())
                    {
                        BranchVertical();
                    }
                    else
                    {
                        findDir = -1;
                    }
                }
                else // x相等
                {
                    if (m_targetPos.y > m_currentPos.y) // 当前在目标下边
                    {
                        if (PeekUp()) // 上边堵住了
                        {
                            BranchHorizontal();
                        }
                        else
                        {
                            // 向上走
                            findDir = 10;
                        }
                    }
                    else if (m_targetPos.y < m_currentPos.y) // 当前在目标上面
                    {
                        if (PeekDown()) //下边堵住了
                        {
                            BranchHorizontal();
                        }
                        else
                        {
                            findDir = -10;
                        }
                    }
                    else if (m_targetPos.x == m_currentPos.x && m_currentPos.y == m_targetPos.y)
                    {
                        isFinded = true;
                    }
                }
            }

            /// <summary>
            /// 优先选择y方向
            /// </summary>
            void PathCorrectY()
            {
                if (m_targetPos.y > m_currentPos.y) // 目标向上
                {
                    if (PeekUp())
                    {
                        BranchHorizontal();
                    }
                    else
                    {
                        findDir = 10;
                    }
                }
                // 目标在当前下边
                else if (m_targetPos.y < m_currentPos.y)
                {
                    //下边堵住了
                    if (PeekDown())
                    {
                        BranchHorizontal();
                    }
                    else
                    {
                        findDir = -10;
                    }
                }
                else // y相等
                {
                    if (m_targetPos.x > m_currentPos.x)
                    {
                        if (PeekRight())
                        {
                            BranchVertical();
                        }
                        else
                        {
                            findDir = 1;
                        }
                    }
                    else if (m_targetPos.x < m_currentPos.x)
                    {
                        if (PeekLeft())
                        {
                            BranchVertical();
                        }
                        else
                        {
                            findDir = -1;
                        }
                    }
                    else if (m_targetPos.x == m_currentPos.x && m_currentPos.y == m_targetPos.y)
                    {
                        isFinded = true;
                    }
                }
            }

            /// <summary>
            /// 分裂左右
            /// </summary>
            void BranchHorizontal()
            {
                bool isLeftBlock = PeekLeft();
                bool isRightBlock = PeekRight();


                // blackList.Add( new GridPos(this.m_currentPos.x,this.m_currentPos.y));
                blackList.Add(AllGridPos[this.m_currentPos.x][this.m_currentPos.y]);
                if (!isLeftBlock && isRightBlock) //左通 右不通
                {
                    findDir = -1;
                }
                else if (isLeftBlock && !isRightBlock) //左不通 右通
                {
                    findDir = 1;
                }
                else if (!isLeftBlock && !isRightBlock) // 双通
                {
                    // 本体向左
                    findDir = -1;
                    // 分裂 拷贝向右
                    FindingNode copy = this.Copy();
                    copy.findDir = 1;
                    findingNodes.Add(copy);
                }
                else if (isLeftBlock && isRightBlock) // 双不通
                {
                    //this.blackList.Add(this.m_currentPos);
                    // 移除当前节点, 或者设置为不用执行的
                    findingNodes.Remove(this);
                }
            }

            void BranchVertical()
            {
                // 分裂吧
                // 分裂前,判断上下是否允许分裂
                bool isUpBlock = PeekUp();
                bool isDownBlock = PeekDown();
                // blackList.Add(new GridPos(this.m_currentPos.x,this.m_currentPos.y));

                try
                {
                    blackList.Add(AllGridPos[this.m_currentPos.x][this.m_currentPos.y]);
                }
                catch (Exception e)
                {
                    //DLog.Detail(this.m_currentPos.x + "   " + this.m_currentPos.y  );
                    Console.WriteLine(e);
                }
                
                if (!isUpBlock && isDownBlock) //上边没有堵住, 下边堵住了
                {
                    // 向上移动
                    this.findDir = 10;
                }
                else if (!isDownBlock && isUpBlock) // 下边没有堵住, 上边堵住了
                {
                    // 向下移动
                    this.findDir = -10;
                }
                else if (isUpBlock && isDownBlock) // 两边都堵住了
                {
                    // 说明这个这个节点是错误节点
                    // 移除本节点 或者 设置本节点为失效节点
                    //this.blackList.Add(this.m_currentPos);
                    this.findingNodes.Remove(this);
                }
                else if (!isUpBlock && !isDownBlock) // 左右都没有堵住
                {
                    // 本体向上
                    this.findDir = 10;
                    // 分裂一个 向下
                    FindingNode copy = this.Copy();
                    copy.findDir = -10;
                    findingNodes.Add(copy);
                }
            }

            /// <summary>
            /// 向右寻路
            /// </summary>
            void LookRight()
            {
                // 向右移动
                this.m_currentPos.x++;
                // 加入已经走过的路
                if (myRoute.Count > 0)
                {
                    myRoute[myRoute.Count - 1].IsRightOpened = true;
                }

                // Route r = new Route(new GridPos(this.m_currentPos.x,this.m_currentPos.y));
                Route r = new Route(AllGridPos[this.m_currentPos.x][this.m_currentPos.y]);
                r.IsLeftOpened = true;
                myRoute.Add(r);
                // 对方向进行校正
                PathCorrect();
            }

            /// <summary>
            /// 向左寻路
            /// </summary>
            void LookLeft()
            {
                // 向左移动
                this.m_currentPos.x--;
                // 加入已经走过的路
                if (myRoute.Count > 0)
                {
                    myRoute[myRoute.Count - 1].IsLeftOpened = true;
                }

                // Route r = new Route(   new GridPos(this.m_currentPos.x,this.m_currentPos.y));
                Route r = new Route(AllGridPos[this.m_currentPos.x][this.m_currentPos.y]);
                r.IsRightOpened = true;
                myRoute.Add(r);
                // 对方向进行校正
                PathCorrect();
            }

            /// <summary>
            /// 向上寻路
            /// </summary>
            void LookUp()
            {
                // 向上移动
                this.m_currentPos.y++;
                // 加入已经走过的路
                if (myRoute.Count > 0)
                {
                    myRoute[myRoute.Count - 1].IsDownOpened = true;
                }

                // Route r = new Route(new GridPos(this.m_currentPos.x,this.m_currentPos.y));
                Route r = new Route(AllGridPos[this.m_currentPos.x][this.m_currentPos.y]);
                r.IsUpOpened = true;
                myRoute.Add(r);
                // 对方向进行校正
                PathCorrect();
            }

            /// <summary>
            /// 向下寻路
            /// </summary>
            void LookDown()
            {
                // 向下移动
                this.m_currentPos.y--;
                // 加入已经走过的路
                if (myRoute.Count > 0)
                {
                    myRoute[myRoute.Count - 1].IsUpOpened = true;
                }

                // Route r = new Route(new GridPos(this.m_currentPos.x,this.m_currentPos.y));
                Route r = new Route(AllGridPos[this.m_currentPos.x][this.m_currentPos.y]);
                r.IsDownOpened = true;
                myRoute.Add(r);
                // 对方向进行校正
                PathCorrect();
            }

            /// <summary>
            /// 查看上面节点是否通
            /// </summary>
            /// <returns></returns>
            bool PeekUp()
            {
                
                if (!IsValidPosition(m_currentPos.x, m_currentPos.y + 1))
                {
                    return false;
                }
                
                // GridPos up = new GridPos(m_currentPos.x, m_currentPos.y + 1);
                GridPos up = AllGridPos[m_currentPos.x][m_currentPos.y + 1];
                if (m_targetPos.x == up.x && m_targetPos.y == up.y)
                {
                    return false;
                }

                cache.Enqueue(up);
                if (up.x < 0 || up.x >= BStartCore.m_Instance.RowCount ||
                    up.y < 0 || up.y >= BStartCore.m_Instance.ColCount)
                {
                    return true;
                }

                if (mapNodes[up.x, up.y] == null ||
                    mapNodes[up.x, up.y].IsBlock ||
                    blackList.Contains(up) ||
                    RouteContainPos(up))
                    // 是障碍或是路或是黑名单
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            /// <summary>
            /// 查看左面节点是否通
            /// </summary>
            /// <returns></returns>
            bool PeekLeft()
            {
                
                if (!IsValidPosition(m_currentPos.x - 1, m_currentPos.y))
                {
                    return false;
                }

                
                GridPos
                    left = AllGridPos[m_currentPos.x - 1][
                        m_currentPos.y]; //  new GridPos(m_currentPos.x - 1, m_currentPos.y);
                cache.Enqueue(left);
                if (m_targetPos.x == left.x && m_targetPos.y == left.y)
                {
                    return false;
                }

                if (left.x < 0 || left.x >= BStartCore.m_Instance.RowCount ||
                    left.y < 0 || left.y >= BStartCore.m_Instance.ColCount)
                {
                    return true;
                }

                if (mapNodes[left.x, left.y] == null ||
                    mapNodes[left.x, left.y].IsBlock ||
                    blackList.Contains(left) ||
                    RouteContainPos(left))
                    // 是障碍或是路或是黑名单
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            /// <summary>
            /// 查看右面节点是否通
            /// </summary>
            /// <returns></returns>
            bool PeekRight()
            {
                
                if (!IsValidPosition(m_currentPos.x + 1, m_currentPos.y))
                {
                    return false;
                }

                
                GridPos right =
                    AllGridPos[m_currentPos.x + 1][
                        m_currentPos.y]; //   new GridPos(m_currentPos.x + 1, m_currentPos.y);
                cache.Enqueue(right);
                if (m_targetPos.x == right.x && m_targetPos.y == right.y)
                {
                    return false;
                }

                if (right.x < 0 || right.x >= BStartCore.m_Instance.RowCount ||
                    right.y < 0 || right.y >= BStartCore.m_Instance.ColCount)
                {
                    return true;
                }

                if (mapNodes[right.x, right.y] == null ||
                    mapNodes[right.x, right.y].IsBlock ||
                    blackList.Contains(right) ||
                    RouteContainPos(right))
                    // 是障碍或是路或是黑名单
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            /// <summary>
            /// 查看下面节点是否通
            /// </summary>
            /// <returns></returns>
            bool PeekDown()
            {

                if (!IsValidPosition(m_currentPos.x, m_currentPos.y - 1))
                {
                    return false;
                }
              
                
                GridPos down = AllGridPos[m_currentPos.x][
                        m_currentPos.y - 1]; //    new GridPos(m_currentPos.x, m_currentPos.y - 1);
                cache.Enqueue(down);
                if (m_targetPos.x == down.x && m_targetPos.y == down.y)
                {
                    return false;
                }

                if (down.x < 0 || down.x >= BStartCore.m_Instance.RowCount ||
                    down.y < 0 || down.y >= BStartCore.m_Instance.ColCount)
                {
                    return true;
                }

                if (mapNodes[down.x, down.y] == null ||
                    mapNodes[down.x, down.y].IsBlock ||
                    blackList.Contains(down) ||
                    RouteContainPos(down)
                ) // 是障碍或是路或是黑名单
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }


            /// <summary>
            /// 是否为合法位置
            /// </summary>
            /// <returns></returns>
            private bool IsValidPosition(int x, int y)
            {
                if (x < 0 || x > AllGridPos.Capacity ||y < 0 || y> AllGridPos.Capacity)
                {
                    return false;
                }

                return true;
            }
            
            private bool RouteContainPos(GridPos p_Pos)
            {
                for (int i = 0; i < myRoute.Count; i++)
                {
                    if (myRoute[i].Pos.x == p_Pos.x &&
                        myRoute[i].Pos.y == p_Pos.y)
                    {
                        return true;
                    }
                }

                return false;
            }

            /// <summary>
            /// 拷贝当前节点
            /// </summary>
            /// <returns></returns>
            public FindingNode Copy()
            {
                FindingNode
                    copy = new FindingNode(); // this.m_targetPos, m_currentPos, findingNodes, blackList, mapNodes, cache);;
                copy.m_targetPos = m_targetPos;
                copy.m_currentPos = m_currentPos;
                copy.findingNodes = findingNodes;
                copy.blackList = blackList;
                copy.mapNodes = mapNodes;
                copy.cache = cache;
                copy.myRoute = new List<Route>();
                Route[] groups = this.myRoute.ToArray();
                for (int i = 0; i < groups.Length; i++)
                {
                    copy.myRoute.Add(groups[i]);
                }

                return copy;
            }
        }

        /// <summary>
        /// 地图节点位置信息
        /// </summary>
        public class GridPos
        {
            public int x;
            public int y;

            public GridPos(int x, int y)
            {
                this.x = x;
                this.y = y;
            }

            public static GridPos operator -(GridPos a, GridPos b)
            {
                return AllGridPos[a.x - b.x][a.y - b.y]; //   new GridPos(a.x - b.x, a.y - b.y);
            }

            public override string ToString()
            {
                return string.Format("({0},{1})", x, y);
            }
        }

        public class Route
        {
            public GridPos Pos;
            public bool IsLeftOpened;
            public bool IsRightOpened;
            public bool IsUpOpened;
            public bool IsDownOpened;

            public Route(GridPos p_Pos, bool leftOpen = false, bool rightOpen = false, bool upOpen = false,
                bool downOpen = false)
            {
                this.Pos = p_Pos;
                this.IsLeftOpened = leftOpen;
                this.IsRightOpened = rightOpen;
                this.IsUpOpened = upOpen;
                this.IsDownOpened = downOpen;
            }

            public override string ToString()
            {
                return Pos.ToString();
            }
        }
    }
}