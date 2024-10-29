using System;

namespace YogiGameCore.Utils
{
    /// <summary>
    /// 地图单个节点信息
    /// </summary>
    public class Node
    {
        internal UInt32 RowPos;
        internal UInt32 ColPos;
        private bool block;
        private Action<bool> ChangedBlock;
        internal Int32 ChildCount;

        public Node(UInt32 pRowPos, UInt32 pColPos, bool p_IsBlock)
        {
            this.RowPos = pRowPos;
            this.ColPos = pColPos;
            block = p_IsBlock;
            ChildCount = 0;
            ChangedBlock += (x) =>
            {
                block = x;
            };
        }

        public bool IsBlock
        {
            get
            {
                return block;
            }
        }

        public void ChangeBlockBool(bool p_IsBlock)
        {
            block = p_IsBlock;
            if (ChangedBlock != null)
            {
                ChangedBlock.Invoke(block);
            }
        }

        public void AddChild()
        {
            ChildCount++;
            ChangedBlock(true);
        }
        public void RemoveChild()
        {
            ChildCount--;
            if (ChildCount <= 0)
            {
                ChildCount = 0;
                ChangedBlock(false);
            }
        }
    }
}
