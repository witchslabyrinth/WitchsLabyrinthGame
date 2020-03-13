/////////////////////////////////////////////////////////////////////////////////////////////////////
//
// Audiokinetic Wwise generated include file. Do not edit.
//
/////////////////////////////////////////////////////////////////////////////////////////////////////

#ifndef __WWISE_IDS_H__
#define __WWISE_IDS_H__

#include <AK/SoundEngine/Common/AkTypes.h>

namespace AK
{
    namespace EVENTS
    {
        static const AkUniqueID CATSWITCHSOUND = 3589155492U;
        static const AkUniqueID DOOR_OPEN = 535830432U;
        static const AkUniqueID FOOTSTEP_PLAYER = 648916573U;
        static const AkUniqueID FOOTSTEPS = 2385628198U;
        static const AkUniqueID LANTERN_LIGHTUP = 2716631521U;
        static const AkUniqueID MUSIC_PLAY = 202194903U;
        static const AkUniqueID OLIVERSWITCHSOUND = 2136553153U;
        static const AkUniqueID ONMENUCLOSE = 1237780649U;
        static const AkUniqueID ONMENUHOVER = 961842423U;
        static const AkUniqueID ONMENUOPEN = 547102971U;
        static const AkUniqueID ONMENUSELECT = 1412352919U;
        static const AkUniqueID POND_SOUND = 661114568U;
        static const AkUniqueID PUZZLE_SOLVED = 1107599359U;
        static const AkUniqueID STAYTHERECAT = 1041426236U;
        static const AkUniqueID STAYTHEREOLIVER = 4294117557U;
        static const AkUniqueID STONES_MOVE = 2574112889U;
        static const AkUniqueID TITLE_MUSIC = 309205993U;
        static const AkUniqueID WIND = 1537061107U;
        static const AkUniqueID WITHINPUZZLEZONE = 3954690858U;
    } // namespace EVENTS

    namespace STATES
    {
        namespace INTERACTION
        {
            static const AkUniqueID GROUP = 3900887599U;

            namespace STATE
            {
                static const AkUniqueID INTERACTING = 3884110025U;
                static const AkUniqueID NOTINTERACTING = 927376446U;
            } // namespace STATE
        } // namespace INTERACTION

        namespace LANTERN_SOLVED
        {
            static const AkUniqueID GROUP = 4247146625U;

            namespace STATE
            {
                static const AkUniqueID NOTSOLVED = 2997843623U;
                static const AkUniqueID SOLVED = 4249573878U;
            } // namespace STATE
        } // namespace LANTERN_SOLVED

        namespace MENU
        {
            static const AkUniqueID GROUP = 2607556080U;

            namespace STATE
            {
                static const AkUniqueID INMENU = 3374585465U;
                static const AkUniqueID OUTOFMENU = 3190209385U;
            } // namespace STATE
        } // namespace MENU

        namespace PLAYING_AS
        {
            static const AkUniqueID GROUP = 4275285196U;

            namespace STATE
            {
                static const AkUniqueID CAT = 983016379U;
                static const AkUniqueID OLIVER = 3589411296U;
            } // namespace STATE
        } // namespace PLAYING_AS

    } // namespace STATES

    namespace SWITCHES
    {
        namespace FOOTSTEPS
        {
            static const AkUniqueID GROUP = 2385628198U;

            namespace SWITCH
            {
                static const AkUniqueID GRASS = 4248645337U;
                static const AkUniqueID WOOD = 2058049674U;
            } // namespace SWITCH
        } // namespace FOOTSTEPS

        namespace MATERIAL
        {
            static const AkUniqueID GROUP = 3865314626U;

            namespace SWITCH
            {
                static const AkUniqueID GRASS = 4248645337U;
                static const AkUniqueID STONE = 1216965916U;
                static const AkUniqueID WOOD = 2058049674U;
            } // namespace SWITCH
        } // namespace MATERIAL

    } // namespace SWITCHES

    namespace GAME_PARAMETERS
    {
        static const AkUniqueID TITLE_MUSIC = 309205993U;
    } // namespace GAME_PARAMETERS

    namespace BANKS
    {
        static const AkUniqueID INIT = 1355168291U;
        static const AkUniqueID MAIN = 3161908922U;
        static const AkUniqueID MUSIC = 3991942870U;
        static const AkUniqueID VOICES = 3313685232U;
    } // namespace BANKS

    namespace BUSSES
    {
        static const AkUniqueID MASTER_AUDIO_BUS = 3803692087U;
    } // namespace BUSSES

    namespace AUDIO_DEVICES
    {
        static const AkUniqueID NO_OUTPUT = 2317455096U;
        static const AkUniqueID SYSTEM = 3859886410U;
    } // namespace AUDIO_DEVICES

}// namespace AK

#endif // __WWISE_IDS_H__
