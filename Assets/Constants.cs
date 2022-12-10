using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util
{
	class Constants
    {
		//ステージのZ軸の最大値
		public static float stageMaxPositionZ = 725.0f;
		//ステージのZ軸の最小値
		public static float stageMinPositionZ = -1119.0f;
		//ステージのX軸の最大値
		public static float stageMaxPositionX = 465.0f;
		//ステージのX軸の最小値
		public static float stageMinPositionX = -1791.0f;
		//通常弾のダメージ量
		public static int normalBulletDamage = 1;
		//スナイパーライフルのダメージ量
		public static int sniperBulletDamage = 5;
		//ロケット弾のダメージ量
		public static int rocketBombDamage = 10;
	}
}
