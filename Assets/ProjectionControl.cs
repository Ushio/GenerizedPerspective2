using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class ProjectionControl : MonoBehaviour
{
    [Range(0.0f, 10.0f)]
    public float _rn = 0.1f;

    [Range(0.0f, 10.0f)]
    public float _rf = 5.0f;

    [Range(0.0f, 100.0f)]
    public float _zn = 1.0f;

    [Range(0.0f, 100.0f)]
    public float _zf = 3.0f;

    [Range(0.0f, 1.0f)]
    public float _ReverseR = 0.0f;

    // Update is called once per frame
    static float Mix(float a, float b, float s)
    {
        return a + (b - a) * s;
    }
    void Update()
    {
        Camera camera = GetComponent<Camera>();

        float w = (float)camera.pixelWidth;
        float h = (float)camera.pixelHeight;

        float rn = Mix(_rn, _rf, _ReverseR);
        float rf = Mix(_rf, _rn, _ReverseR);
        float zn = _zn;
        float zf = _zf;

        float f = rn * zf - rf * zn;
        float b = zf - zn;
        float a = h / w * b;
        float e = rn - rf;
        float c = -rn - rf;
        float d = -rn * zf - rf * zn;

        Matrix4x4 m = new Matrix4x4();
        m.m00 = a;
        m.m11 = b;
        m.m22 = c;
        m.m33 = f;

        m.m23 = d;
        m.m32 = e;

        camera.projectionMatrix = m;
    }

    void OnDrawGizmos()
    {
        Camera camera = GetComponent<Camera>();
        float w = (float)camera.pixelWidth;
        float h = (float)camera.pixelHeight;

        var o = transform.position;

        float rn = Mix(_rn, _rf, _ReverseR);
        float rf = Mix(_rf, _rn, _ReverseR);
        float zn = _zn;
        float zf = _zf;

        // a b
        // c d
        var an = o + transform.forward * zn + transform.up * rn + transform.right * (-rn / h * w);
        var bn = o + transform.forward * zn + transform.up * rn + transform.right * (+rn / h * w);
        var cn = o + transform.forward * zn + transform.up * -rn + transform.right * (-rn / h * w);
        var dn = o + transform.forward * zn + transform.up * -rn + transform.right * (+rn / h * w);

        Gizmos.DrawLine(an, bn);
        Gizmos.DrawLine(bn, dn);
        Gizmos.DrawLine(dn, cn);
        Gizmos.DrawLine(cn, an);

        var af = o + transform.forward * zf + transform.up * rf + transform.right * (-rf / h * w);
        var bf = o + transform.forward * zf + transform.up * rf + transform.right * (+rf / h * w);
        var cf = o + transform.forward * zf + transform.up * -rf + transform.right * (-rf / h * w);
        var df = o + transform.forward * zf + transform.up * -rf + transform.right * (+rf / h * w);

        Gizmos.DrawLine(af, bf);
        Gizmos.DrawLine(bf, df);
        Gizmos.DrawLine(df, cf);
        Gizmos.DrawLine(cf, af);

        Gizmos.DrawLine(an, af);
        Gizmos.DrawLine(bn, bf);
        Gizmos.DrawLine(cn, cf);
        Gizmos.DrawLine(dn, df);
    }
}
