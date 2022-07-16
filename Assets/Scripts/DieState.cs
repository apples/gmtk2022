
using System;
using UnityEngine;

public class DieState
{
    public class FaceState
    {
        public DieFaceEffect effect;
    }

    public FaceState[] faces;

    // Y+, Z+, X+, X-, Z-, Y-
    public int[] faceOrder;

    public FaceState TopFace => faces[faceOrder[0]];

    public DieState(DieData dieData)
    {
        faces = new FaceState[6];
        faceOrder = new int[6] { 0, 1, 2, 3, 4, 5 };

        for (var i = 0; i < 6; ++i)
        {
            faces[i] = new FaceState { effect = dieData.faces[i].effect };
        }
    }

    public void Spin(Dir dir)
    {
        Span<int> loop = stackalloc int[4];
        loop[0] = 0;
        loop[2] = 5;

        switch (dir)
        {
            case Dir.Up:
                loop[1] = 1;
                loop[3] = 4;
                break;
            case Dir.Down:
                loop[1] = 4;
                loop[3] = 1;
                break;
            case Dir.Right:
                loop[1] = 2;
                loop[3] = 3;
                break;
            case Dir.Left:
                loop[1] = 3;
                loop[3] = 2;
                break;
            default:
                throw new InvalidOperationException($"Unkown dir {dir}");
        }

        var tmp = faceOrder[loop[0]];
        faceOrder[loop[0]] = faceOrder[loop[1]];
        faceOrder[loop[1]] = faceOrder[loop[2]];
        faceOrder[loop[2]] = faceOrder[loop[3]];
        faceOrder[loop[3]] = tmp;
    }
}
