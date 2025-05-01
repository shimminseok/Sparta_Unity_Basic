using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MiniGameTable", menuName = "Scriptable Objects/MiniGameTable")]
public class MiniGameTable : BaseTable<MiniGameData>
{
    public override void CreateTable()
    {
        base.CreateTable();
        foreach (var data in dataList)
        {
            dataDic[data.MiniGameID] = data;
        }
    }
}