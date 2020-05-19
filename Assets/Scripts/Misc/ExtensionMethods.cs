using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods{
   
   public static int GridPositionX(this Transform T){
       return (int) Mathf.Floor(T.position.x);
   }
   public static int GridPositionY(this Transform T){
       return (int) Mathf.Floor(T.position.y);
   }
}
