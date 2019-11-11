using UnityEngine;
using System.Collections;

[System.Serializable]
[CreateAssetMenu(fileName = "SoundTrack")]
public class SoundTrack : ScriptableObject
{   

    public SoundType type;
    public AudioClip clip;

    [Range(0, 1)]
    public float volume = 1;
    public bool loop = false;
}

public enum SoundType
{
    NONE = 0,

    // Player
    PUNCH = 100,
    GROUND_POUND = 101,
    TAKE_DAMAGE = 102,
    STEP_1 = 103,
    STEP_2 = 104,
    MEGA_READY = 105,
    PLAYER_DEATH = 106,
    HEAL = 107,
    DASH = 108,
    FOOTSTEPS = 109,

    // Grail
    BUFF = 200,
    DRINK = 201,
    FILL = 202,
    DROP = 203,
    PICKUP = 204,
    GRAIL_SPAWN = 205,
    ICE_BASIC = 206,
    ICE_MEGA = 207,
    MECH_BASIC = 208,
    MECH_MEGA = 209,
    VAMPIRE_BASIC = 210,
    VAMPIRE_MEGA = 211,
    LIGHT_BASIC = 212,
    LIGHT_HEAVY = 213,
    GRAIL_SWITCH = 214,
    ELECTRIC_BASIC = 215,
    ELECTRIC_MEGA = 216,

    // Enemy
    BAT_SHOT = 300,
    BAT_DEATH = 301,
    STEAMON_ATTACK = 302,
    STEAMON_DEATH = 303,
    RUSH_ATTACK = 304,
    RUSH_DEATH = 305,

    //Boss
    ASMODEUS_SLAM = 400,
    ASMODEUS_FIRE_BELCH = 401,
    ASMODEUS_DEATH = 402,

    //Music
    LEVEL_1 = 500,
    LEVEL_2 = 501,
    LEVEL_3 = 502,

    MAIN_MENU = 503,
    AMBIENT = 504,
}
