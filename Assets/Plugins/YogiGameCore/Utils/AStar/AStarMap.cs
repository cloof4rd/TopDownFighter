using System.Collections.Generic;
using System.Linq;
using YogiGameCore.Log;

namespace YogiGameCore.Utils.AStar
{
    /// <summary>
    /// A*寻路工具, 依赖了优先队列功能
    /// </summary>
    public class AStarMap
    {
        public NodeBase[,] Map;
        public int width { get; private set; }
        public int height { get; private set; }

        /// <summary>
        /// 初始寻路地图
        /// </summary>
        /// <param name="map">这里入参需要0:障碍物 1:通路 的一个二维数组</param>
        public void Init(int[,] map)
        {
            this.width = map.GetLength(0);
            this.height = map.GetLength(1);
            // 卷积算法，从障碍物(1处)开始扩散
            Convolution(1, ref map);
            this.Map = new NodeBase[this.width, this.height];
            for (int x = 0; x < this.width; x++)
            {
                for (int y = 0; y < this.height; y++)
                {
                    this.Map[x, y] = new NodeBase(x, y, map[x, y]);
                }
            }
        }
        /// <summary>
        /// 寻路
        /// </summary>
        /// <param name="findSize">寻路大小,值越小路线越贴近障碍物</param>
        /// <param name="startX">开始寻路位置X</param>
        /// <param name="startY">开始寻路位置Y</param>
        /// <param name="endX">终点寻路位置X</param>
        /// <param name="endY">终点寻路位置Y</param>
        /// <returns></returns>
        public List<NodeBase> PathFind(int findSize, int startX, int startY, int endX, int endY)
        {
            //计算合法路径
            var legalStart = GetClosetWalkablePos(findSize, ref startX, ref startY);
            var legalEnd = GetClosetWalkablePos(findSize, ref endX, ref endY);
            if (!legalStart || !legalEnd)
            {
                return null;
            }

            PriorityQueue<NodeBase> toSearch = new PriorityQueue<NodeBase>(8); // 需要检索的

            NodeBase startNode = Map[startX, startY];
            NodeBase endNode = Map[endX, endY];
            List<NodeBase> processed = new List<NodeBase>(); // 已经查询过的节点

            startNode.G = 0; // 消耗
            startNode.UpdateH(endNode);
            toSearch.Enqueue(startNode);

            while (toSearch.Count > 0)
            {
                NodeBase currentNode = toSearch.Dequeue();

                processed.Add(currentNode);
                if (ReferenceEquals(currentNode, endNode))
                {
                    List<NodeBase> resultPath = new List<NodeBase>();
                    resultPath.Add(currentNode);
                    currentNode = currentNode.Connection;

                    while (!ReferenceEquals(currentNode, startNode) && !resultPath.Contains(currentNode) &&
                           currentNode != null)
                    {
                        resultPath.Add(currentNode);
                        currentNode = currentNode.Connection;
                        if (resultPath.Count > 1000)
                        {
                            LogCore.Error("路径过长");
                            return null;
                        }
                    }

                    return resultPath;
                }
                else
                {
                    //查找邻居
                    CheckNeighbor(currentNode.x - 1, currentNode.y);
                    CheckNeighbor(currentNode.x + 1, currentNode.y);
                    CheckNeighbor(currentNode.x, currentNode.y + 1);
                    CheckNeighbor(currentNode.x, currentNode.y - 1);

                    void CheckNeighbor(int x, int y)
                    {
                        if (x < 0 || x >= width || y < 0 || y >= height)
                        {
                            // 此处无邻居
                        }
                        else
                        {
                            NodeBase neighbor = Map[x, y];
                            bool inSearch = toSearch.Contains(neighbor);
                            bool inProcessed = processed.Contains(neighbor);
                            var costToNeighbor = currentNode.G + currentNode.GetDistance(neighbor.x, neighbor.y);

                            if (!inSearch
                                && !inProcessed
                                && neighbor.NodeType >= findSize
                                || costToNeighbor < neighbor.G)
                            {
                                neighbor.G = costToNeighbor;
                                neighbor.SetConnection(currentNode);

                                if (!inSearch)
                                {
                                    neighbor.UpdateH(endNode);
                                    toSearch.Enqueue(neighbor);
                                }
                            }
                        }
                    }
                }
            }

            return null;
        }
        
        /// <summary>
        /// 卷积算法 核心=(周围四个格子的最小值)+1
        /// </summary>
        /// <param name="currentVal"></param>
        /// <param name="map"></param>
        /// <param name="limit"></param>
        private void Convolution(int currentVal, ref int[,] map, int limit = 50)
        {
            if (limit <= 0)
            {
                LogCore.Error("卷积算法递归超出数量限制");
                return;
            }

            int[,] mapCopy;
            mapCopy = (int[,])map.Clone();
            // map.CopyTo(mapCopy, 0);
            int nextConvolutionCore = currentVal + 1;
            bool isUpdate = false;
            for (int x = 0; x < this.width; x++)
            {
                for (int y = 0; y < this.height; y++)
                {
                    int nodeType = mapCopy[x, y];
                    if (currentVal == nodeType)
                    {
                        List<int> all = new List<int>();
                        //var top =
                        GetValWithoutBounds(0, 1);
                        //var down =
                        GetValWithoutBounds(0, -1);
                        //var left =
                        GetValWithoutBounds(-1, 0);
                        //var right =
                        GetValWithoutBounds(1, 0);

                        //var leftTop =
                        GetValWithoutBounds(-1, 1);
                        //var leftDown =
                        GetValWithoutBounds(-1, -1);
                        //var rightTop =
                        GetValWithoutBounds(1, 1);
                        //var rightDown =
                        GetValWithoutBounds(1, -1);

                        // var minVal = Math.Min(top, Math.Min(down, Math.Min(left, right)));
                        map[x, y] = all.Min() + 1;
                        isUpdate = true;

                        void GetValWithoutBounds(int offsetX, int offsetY)
                        {
                            var newX = x + offsetX;
                            var newY = y + offsetY;
                            if (newX > this.width - 1 || newX < 0 ||
                                newY > this.height - 1 || newY < 0)
                            {
                                return;
                            }
                            else
                            {
                                all.Add(mapCopy[newX, newY]);
                            }
                        }
                    }
                }
            }

            if (isUpdate)
            {
                Convolution(nextConvolutionCore, ref map, --limit);
            }
        }

        /// <summary>
        /// 查找最接近终点的可移动位置
        /// </summary>
        /// <param name="findSize"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private bool GetClosetWalkablePos(int findSize, ref int x, ref int y)
        {
            // 范围检测
            if (x < 0)
            {
                x = 0;
            }
            else if (x >= width)
            {
                x = width - 1;
            }

            if (y < 0)
            {
                y = 0;
            }
            else if (y >= height)
            {
                y = height - 1;
            }

            // 可行走位置检测
            if (Map[x, y].NodeType < findSize)
            {
                NodeBase targetNode = null;
                List<NodeBase> processed = new List<NodeBase>();
                List<NodeBase> inSearch = new List<NodeBase>();
                NodeBase currentNode = Map[x, y];

                inSearch.Add(currentNode);
                int LimitCount = 100;
                while (inSearch.Count > 0 && targetNode == null)
                {
                    if (--LimitCount < 0)
                    {
                        LogCore.Warning("递归寻找最近可通行道路失败");
                        break;
                    }

                    currentNode = inSearch.First();
                    inSearch.Remove(currentNode);
                    processed.Add(currentNode);

                    CheckNeighborWalkAble(currentNode.x - 1, currentNode.y);
                    CheckNeighborWalkAble(currentNode.x + 1, currentNode.y);
                    CheckNeighborWalkAble(currentNode.x, currentNode.y - 1);
                    CheckNeighborWalkAble(currentNode.x, currentNode.y + 1);

                    void CheckNeighborWalkAble(int xi, int yi)
                    {
                        LogCore.Log($"test:({xi},{yi}):{Map[xi, yi].NodeType}");
                        if (xi < 0 || xi >= width || yi < 0 || yi >= height)
                        {
                            // 此处无邻居
                        }
                        else
                        {
                            var neighbor = Map[xi, yi];
                            var isProcessed = processed.Contains(neighbor);
                            if (!isProcessed)
                            {
                                if (neighbor.NodeType >= findSize)
                                {
                                    targetNode = neighbor;
                                }
                                else
                                {
                                    inSearch.Add(neighbor);
                                }
                            }
                        }
                    }
                }

                if (targetNode != null)
                {
                    x = targetNode.x;
                    y = targetNode.y;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }
    }
}