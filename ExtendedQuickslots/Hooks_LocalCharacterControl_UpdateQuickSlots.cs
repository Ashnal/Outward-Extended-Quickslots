namespace ExtendedQuickslots
{
    public static partial class Hooks
    {
        public static void LocalCharacterControl_UpdateQuickSlots(On.LocalCharacterControl.orig_UpdateQuickSlots orig, LocalCharacterControl self)
        {
            orig(self);

            if (self.Character != null && self.Character.QuickSlotMngr != null)
            {
                int playerID = self.Character.OwnerPlayerSys.PlayerID;

                if (ControlsInput.m_playerInputManager[playerID].GetButtonDown("Sit_Emote"))
                {
                    self.Character.CastSpell(Character.SpellCastType.Sit, self.Character.gameObject, Character.SpellCastModifier.Immobilized, 1, -1f);
                }
                else if (ControlsInput.m_playerInputManager[playerID].GetButtonDown("Alternate_Idle_Emote"))
                {
                    self.Character.CastSpell(Character.SpellCastType.IdleAlternate, self.Character.gameObject, Character.SpellCastModifier.Immobilized, 1, -1f);
                }

                for (var x = 0; x < ExtendedQuickslots.NumberOfExtraSlotsToAdd.Value; ++x)
                {
                    bool inputRecieved = ControlsInput.m_playerInputManager[playerID].GetButtonDown(string.Format("QS_Instant{0}", x + 12));
                    //Debug.Log(string.Format("Checking QS_Instant{0}: {1}", x+12, inputRecieved));
                    if (inputRecieved)
                    {
                        ExtendedQuickslots.Logger.LogDebug("UpdateQuickSlotsPatch() QS_Instant" + (x + 12));
                        self.Character.QuickSlotMngr.QuickSlotInput(x + 11);
                        break;
                    }
                }
            }
        }
    }
}
