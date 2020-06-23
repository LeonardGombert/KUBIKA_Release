using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kubika.Game
{
    public static class _DirectionCustom 
    {
        public static int rotationState;// { get; set; }

        public static int matrixLengthDirection;

        /// WORLD MODE
        public static int forward => (matrixLengthDirection * matrixLengthDirection);
        
        public static int backward => -forward;

        public static int up => 1;
        public static int down => -up;

        public static int right => matrixLengthDirection;
        public static int left => -right;



        /// LOCAL MODE
        public static int fixedForward => rotationState == 0 ? (matrixLengthDirection * matrixLengthDirection) :
                                (rotationState == 1 ? matrixLengthDirection :
                                (rotationState == 2 ? 1 : 0));
        public static int fixedBackward => -forward;
        public static int fixedUp => rotationState == 0 ? 1 :
                                        (rotationState == 1 ? (matrixLengthDirection * matrixLengthDirection) :
                                        (rotationState == 2 ? matrixLengthDirection : 0));
        public static int fixedDown => -up;
        public static int fixedRight => rotationState == 0 ? matrixLengthDirection :
                                        (rotationState == 1 ? 1 :
                                        (rotationState == 2 ? (matrixLengthDirection * matrixLengthDirection) : 0));
        public static int fixedLeft => -right;

        public static int LocalScanner(FacingDirection facingDirection)
        {
            if (facingDirection == FacingDirection.up)
            {
                return fixedUp;
            }
            else if (facingDirection == FacingDirection.down)
            {
                return fixedDown;
            }
            else if (facingDirection == FacingDirection.forward)
            {
                return fixedForward;
            }
            else if (facingDirection == FacingDirection.backward)
            {
                return fixedBackward;
            }
            else if (facingDirection == FacingDirection.right)
            {
                return fixedRight;
            }
            else if (facingDirection == FacingDirection.left)
            {
                return fixedLeft;
            }
            else
            {
                return 69;
            }
        }

        /// LOCAL VECTOR
        public static Vector3 vectorForward => rotationState == 0 ? Vector3.forward :
                                (rotationState == 1 ? Vector3.up :
                                (rotationState == 2 ? Vector3.right : Vector3.zero));

        public static Vector3 vectorBack => -vectorForward;
        public static Vector3 vectorLeft => rotationState == 0 ? Vector3.left :
                                        (rotationState == 1 ? Vector3.back :
                                        (rotationState == 2 ? Vector3.up : Vector3.zero));
        public static Vector3 vectorRight => -vectorLeft;
        public static Vector3 vectorUp => rotationState == 0 ? Vector3.up :
                                        (rotationState == 1 ? Vector3.right :
                                        (rotationState == 2 ? Vector3.forward : Vector3.zero));
        public static Vector3 vectorDown => -vectorUp;
        


    }
}
