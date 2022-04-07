using System;

namespace KawaggyMod.Core.Helpers
{
    //These are abominations
    public static partial class KawaggyHelper
    {
        /// <summary>
        /// Eases in and out between a value of 0 and 1
        /// </summary>
        /// <param name="time">The time to ease in and out </param>
        /// <param name="alpha">The total "strength" of the easing, keep on 2 for proper easing, 3 for funny </param>
        /// <param name="clamp">If the time value is not between 0 or 1 it returns <see cref="float.NaN"/></param>
        /// <returns></returns>
        public static float EaseInOut(float time, float alpha, bool clamp)
        {
            if (clamp)
            {
                if (time < 0)
                {
                    return float.NaN;
                }

                if (time > 1)
                {
                    return float.NaN;
                }
            }

            float square = time * time;
            return square / (alpha * (square - time) + 1.0f);
        }

        /// <summary>
        /// <inheritdoc cref="EaseInOut(float, float, bool)"/>and does not clamp time
        /// </summary>
        /// <param name="time"><inheritdoc cref="EaseInOut(float, float, bool)" path="/param[@name='time']"/></param>
        /// <param name="alpha"><inheritdoc cref="EaseInOut(float, float, bool)" path="/param[@name='alpha']"/></param>
        /// <returns></returns>
        public static float EaseInOut(float time, float alpha)
        {
            return EaseInOut(time, alpha, false);
        }

        /// <summary>
        /// <inheritdoc cref="EaseInOut(float, float)"/>with the default value of alpha
        /// </summary>
        /// <param name="time"><inheritdoc cref="EaseInOut(float, float)" path="/param[@name='time']"/></param>
        /// <returns></returns>
        public static float EaseInOut(float time)
        {
            return EaseInOut(time, 2.0f);
        }
    }
}
