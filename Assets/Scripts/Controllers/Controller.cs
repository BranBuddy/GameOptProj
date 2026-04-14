/*
   Helper function that is used to easily find controller
*/

using UnityEngine;

public class Controller : MonoBehaviour
   
{
   private void OnEnable()
   {
      Debug.Log($"[Controller] Enabled on {gameObject.name}");
   }

   private void OnDisable()
   {
      Debug.Log($"[Controller] Disabled on {gameObject.name}");
   }
   public InputController inputController = null;
}
