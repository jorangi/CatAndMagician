using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IntWaveDictionary : SerializableDictionary <int, Wave>{ }
[System.Serializable]
public class StrIntDictionary : SerializableDictionary <string, int>{ }
[System.Serializable]
public class StrBoolDictionary : SerializableDictionary <string, bool>{ }
[System.Serializable]
public class StrStrDictionary : SerializableDictionary <string, string>{ }

[System.Serializable]
public class StrGameObjDictionary : SerializableDictionary<string, GameObject> { }

[System.Serializable]
public class StrBspawnerDictionary : SerializableDictionary <string, BulletSpawner>{ }
[System.Serializable]
public class StrBdataDictionary : SerializableDictionary <string, BulletData>{ }
