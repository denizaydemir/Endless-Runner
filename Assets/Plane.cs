using UnityEngine;
using System.Collections;

public class Plane {

    private int _index;
    private float _zPosition;
    private GameObject _thisGameObject;

    public Plane(int _index, float _zPosition, GameObject _thisGameObject)
    {
        this._index = _index;
        this._zPosition = _zPosition;
        this._thisGameObject = _thisGameObject;
    }


    public int Index
    {
        get
        {
            return _index;
        }

        set
        {
            _index = value;
        }
    }

   

    public float ZPosition
    {
        get
        {
            return _zPosition;
        }

        set
        {
            _zPosition = value;
        }
    }

    public GameObject ThisGameObject
    {
        get
        {
            return _thisGameObject;
        }

        set
        {
            _thisGameObject = value;
        }
    }
}
