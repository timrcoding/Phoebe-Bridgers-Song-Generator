using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Instruments", menuName = "ScriptableObjects/InstrumentDatabase")]
public class InstrumentDictionary : ScriptableObject
{
    public List<InstrumentEntry> InstrumentLookupTable;
}

[System.Serializable]
public class InstrumentEntry
{
    public InstrumentType m_InstrumentType;
    public List<ChordToFMODSTRING> m_ChordToFMODString;
}

[System.Serializable]
public struct ChordToFMODSTRING
{
    public ChordType SongChord;
    [FMODUnity.EventRef]
    public string FMODLookup;
}


