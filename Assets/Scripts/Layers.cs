using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Layers
{
    private static int _default = -1;
    public static int Default
    {
        get
        {
            if (_default == -1) _default = LayerMask.NameToLayer("Default");
            return _default;
        }
    }

    private static int _transparentFX = -1;
    public static int TransparentFX
    {
        get
        {
            if (_transparentFX == -1) _transparentFX = LayerMask.NameToLayer("TransparentFX");
            return _transparentFX;
        }
    }

    private static int _water = -1;
    public static int Water
    {
        get
        {
            if (_water == -1) _water = LayerMask.NameToLayer("Water");
            return _water;
        }
    }

    private static int _ui = -1;
    public static int UI
    {
        get
        {
            if (_ui == -1) _ui = LayerMask.NameToLayer("UI");
            return _ui;
        }
    }

    private static int _finish = -1;
    public static int Finish
    {
        get
        {
            if (_finish == -1) _finish = LayerMask.NameToLayer("Finish");
            return _finish;
        }
    }

    private static int _ball = -1;
    public static int Ball
    {
        get
        {
            if (_ball == -1) _ball = LayerMask.NameToLayer("Ball");
            return _ball;
        }
    }

    private static int _enemy = -1;
    public static int Enemy
    {
        get
        {
            if (_enemy == -1) _enemy = LayerMask.NameToLayer("Enemy");
            return _enemy;
        }
    }
}