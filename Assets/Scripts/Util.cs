using UnityEngine;

public class Util {

    public static void setLayerRecursively(GameObject _obj,int newLayer)
    {
        if(_obj == null){
            return;
        }
        foreach (Transform  _child in _obj.transform)
        {
            if(_child == null){
                continue;
            }
            setLayerRecursively(_child.gameObject, newLayer);
        }
    }
}
