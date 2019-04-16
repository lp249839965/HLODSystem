﻿using System.Collections;
using System.Collections.Generic;
using Unity.HLODSystem.SpaceManager;
using Unity.HLODSystem.Streaming;
using UnityEngine;

namespace Unity.HLODSystem
{
    public class ActiveHLODTreeNodeManager
    {
        private List<HLODTreeNode> m_activeTreeNode = new List<HLODTreeNode>();

        public void UpdateActiveNodes()
        {
            for (int i = 0; i < m_activeTreeNode.Count; ++i)
            {
                m_activeTreeNode[i].Update();
            }
        }

        public void Activate(HLODTreeNode node)
        {
            m_activeTreeNode.Add(node);
        }

        public void Deactivate(HLODTreeNode node)
        {
            if (m_activeTreeNode.Remove(node) == true)
            {
                //Succeed to remove, that means the child also activated.
                //so, we should remove child own.
                if (node.ChildNodes != null)
                {
                    for (int i = 0; i < node.ChildNodes.Count; ++i)
                    {
                        Deactivate(node.ChildNodes[i]);
                    }
                }
            }
        }
    }

}