using UnityEngine;
using System.Collections;

public class InteractionManager : MonoBehaviour
{
    

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator DisableThenDestroy(GameObject obj)
    {
        this.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f); // Wait for 0.5 seconds before destroying the object
        Destroy(this.gameObject);
    }
}
