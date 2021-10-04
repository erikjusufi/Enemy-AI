using Unity.FPS.Gameplay;
using UnityEngine;

namespace Binx
{
    /// <summary>
    /// Custom weapons manager script for handling single melee weapon and shield.
    /// </summary>
    public class PlayerWeaponsManager : MonoBehaviour
    {
        [SerializeField] private Sword sword;
        [SerializeField] private Shield shield;
        
        public bool IsSwinging => sword.IsSwinging;
        public bool IsBlocking => shield.IsBlocking;

        private PlayerInputHandler inputHandler;
        private bool blockInputHeld;
        private bool swingInputHeld;
        
        private void Awake()
        {
            inputHandler = GetComponent<PlayerInputHandler>();
        }

        private void Update()
        {
            // Recycle aim input for block
            blockInputHeld = inputHandler.GetAimInputHeld() && !sword.IsSwinging;
            
            // Recycle fire input for swing
            swingInputHeld = inputHandler.GetFireInputHeld() && !shield.IsBlocking;

            sword.HandleInput(swingInputHeld);
            shield.HandleInput(blockInputHeld && !swingInputHeld);
        }
    }
}