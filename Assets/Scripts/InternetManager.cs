using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class InternetManager : NetworkManager
{
   
    public string roomName = "room1";
    
    public void Start () {

        StartMatchMaker();

        matchMaker.ListMatches(0, 20, "", OnMatchList);
    }

    void CreateMatch(string newMatchName)
    {
        matchName = newMatchName;
        matchMaker.CreateMatch(matchName, 2, true, "", OnMatchCreate);
    }

    void JoinMatch(MatchDesc match)
    {
        matchName = match.name;
        matchSize = (uint)match.currentSize;
        matchMaker.JoinMatch(match.networkId, "", OnMatchJoined);
    }

    public override void OnMatchList(ListMatchResponse matchList)
    {
        base.OnMatchList(matchList);

        // auto join room if found
        foreach (MatchDesc match in matchList.matches)
        {
            Debug.Log("Match "+match.name + " found");
            if (match.name == roomName)
            {
                JoinMatch(match);
                return;
            }
        }

        // otherwise create room
        CreateMatch(roomName);

    }


}
