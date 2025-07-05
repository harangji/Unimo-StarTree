using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AddressableKeyCtrl
{
    static readonly private int totalCha = 13;
    static readonly private int totalEq = 23;
    static readonly private int commonEq = 10;

    static public void ChangeCharacterIdx(int diff, ref int newidx)
    {
        newidx += diff;
        if (newidx > totalCha) { newidx = 1; }
        if (newidx < 1) { newidx = totalCha; }
    }
    static public void ChangeEquipIdx(int diff, ref int newidx)
    {
        newidx += diff;
        if (newidx > totalEq) { newidx = 1; }
        if (newidx < 1) { newidx = totalEq; }
    }
    static public string ChaAssetKey_Lobby(int idx)
    {
        return $"CH{idx:D3}L";
    }
    static public string EqAssetKey_Lobby(int idx)
    {
        if (idx > totalEq - commonEq)
        {
            int cidx = idx- totalCha;
            return $"EQC{cidx:D3}L";
        }
        else { return $"EQ{idx:D3}L"; }
    }
    static public string ChaAssetKey_Play(int idx)
    {
        return $"CH{idx:D3}";
    }
    static public string EqAssetKey_Play(int idx)
    {
        if (idx > totalEq - commonEq)
        {
            int cidx = idx - totalCha;
            return $"EQC{cidx:D3}";
        }
        else { return $"EQ{idx:D3}"; }
    }
}
