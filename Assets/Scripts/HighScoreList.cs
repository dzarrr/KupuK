using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreList:IComparable<HighScoreList> {

    public int Score { get; set; }
    public string Name { get; set; }
    public int Id { get; set; }

	public HighScoreList(int id, string name, int score )
    {
        this.Id = id;
        this.Name = name;
        this.Score = score;
    }

    public int CompareTo(HighScoreList other)
    {
        throw new NotImplementedException();
    }
}
