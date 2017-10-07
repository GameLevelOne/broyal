using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetData {
	public string name;
	public int id;
	public string imageUrl;
	public string modelName;
	public string description;
	public int price;
	public string rank;
	public string skillDescription;
	public int exp;
	public int nextRankExp;
	public bool equipped;

	public PetData() {
		name = "";
		id = 0;
		imageUrl = "";
		modelName = "";
		description = "";
		price = 0;
		rank = "E";
		skillDescription = "";
		exp = 0;
		nextRankExp = 0;
		equipped = false;
	}

	public void InitHeader(string _name, string _imageUrl, string _rank, int _exp, int _nextRankExp) {
		name = _name;
		imageUrl = _imageUrl;
		rank = _rank;
		exp = _exp;
		nextRankExp = _nextRankExp;
	}

	public void InitProfile(string _name, int _id, string _imageUrl, string _modelName, string _description, string _rank, int _exp, int _nextRankExp, string _skillDescription) {
		name = _name;
		id = _id;
		imageUrl = _imageUrl;
		modelName = _modelName;
		description = _description;
		rank = _rank;
		skillDescription = _skillDescription;
		exp = _exp;
		nextRankExp = _nextRankExp;
	}

	public void InitMiniProfile(string _name, int _id, string _imageUrl, string _modelName, string _description, string _rank, bool _equipped) {
		name = _name;
		id = _id;
		imageUrl = _imageUrl;
		modelName = _modelName;
		description = _description;
		rank = _rank;
		equipped = _equipped;
	}

	public void InitShopProfile(int _id, string _imageUrl, string _modelName, int _price, string _description) {
		id = _id;
		imageUrl = _imageUrl;
		modelName = _modelName;
		description = _description;
		price = _price;
	}

	public void UpdateExp(string _rank, int _exp, int _nextRankExp) {
		rank = _rank;
		exp = _exp;
		nextRankExp = _nextRankExp;
	}
}
