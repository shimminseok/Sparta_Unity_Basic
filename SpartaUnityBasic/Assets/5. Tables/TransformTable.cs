using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TransformTable", menuName = "Scriptable Objects/TransformTable")]
public class TransformTable : BaseTable<TransformData>
{
    override public void CreateTable()
    {
        base.CreateTable();
        foreach (var data in dataList)
        {
            dataDic[data.TransformID] = data;
        }
    }
}