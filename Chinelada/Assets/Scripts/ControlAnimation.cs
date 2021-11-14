 using UnityEngine;
 using UnityEngine.UI;
 

// ISSO FOI APENAS UM TESTE
 

 public class ControlAnimation : MonoBehaviour
 {
     private Animator anim;
     public Slider slider;   //Assign the UI slider of your scene in this slot 
 
     // Use this for initialization
     void Start()
     {
         anim = GetComponent<Animator>();
         anim.speed = 0; 
     }
 
     // Update is called once per frame
     void Update()
     {
         anim.Play("JustTest", -1, slider.normalizedValue);
         // print(slider.normalizedValue);
     }
 }