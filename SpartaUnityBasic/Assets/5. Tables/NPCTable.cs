using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCTable", menuName = "Scriptable Objects/NPCTable")]
public class NPCTable : BaseTable<NPCData>
{
    public override void CreateTable()
    {
        base.CreateTable();
        foreach (var data in dataList)
        {
            dataDic[data.NPCID] = data;
        }
    }
}