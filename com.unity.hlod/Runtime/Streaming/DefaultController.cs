﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.HLODSystem.Streaming
{
    [RequireComponent(typeof(HLOD))]
    public class DefaultController : ControllerBase
    {
        [SerializeField]
        private List<GameObject> m_gameObjectList = new List<GameObject>();
        [SerializeField]
        private List<HLODMesh> m_hlodMeshes = new List<HLODMesh>();

        private List<GameObject> m_createdHlodMeshObjects = new List<GameObject>();

        public int AddHighObject(GameObject gameObject)
        {
            int id = m_gameObjectList.Count;
            m_gameObjectList.Add(gameObject);
            return id;
        }

        public int AddLowObject(HLODMesh hlodMesh)
        {
            int id = m_hlodMeshes.Count;
            m_hlodMeshes.Add(hlodMesh);
            return id;
        }

        public override IEnumerator GetHighObject(int id, Action<GameObject> callback)
        {
            if (callback != null)
            {
                callback(m_gameObjectList[id]);
            }
            yield break;
        }

        public override IEnumerator GetLowObject(int id, Action<GameObject> callback)
        {
            if (callback != null)
            {
                callback(m_createdHlodMeshObjects[id]);
            }
            yield break;
        }

        private HLOD m_hlod;
        private GameObject m_hlodMeshesRoot;
        void Start()
        {
            m_hlod = GetComponent<HLOD>();
            m_hlodMeshesRoot = new GameObject("HLODMeshesRoot");
            m_hlodMeshesRoot.transform.SetParent(m_hlod.transform);
            
            for (int i = 0; i < m_hlodMeshes.Count; ++i)
            {
                GameObject go = new GameObject(m_hlodMeshes[i].name);

                go.AddComponent<MeshFilter>().sharedMesh = m_hlodMeshes[i].ToMesh();
                go.AddComponent<MeshRenderer>().material = m_hlodMeshes[i].Material;
                go.transform.parent = m_hlodMeshesRoot.transform;

                go.SetActive(false);

                m_createdHlodMeshObjects.Add(go);
            }

            for (int i = 0; i < m_gameObjectList.Count; ++i)
            {
                m_gameObjectList[i].SetActive(false);
            }
        }

        public override void ReleaseHighObject(int id)
        {
            m_gameObjectList[id].SetActive(false);
        }

        public override void ReleaseLowObject(int id)
        {
            m_createdHlodMeshObjects[id].SetActive(false);
        }
    }

}