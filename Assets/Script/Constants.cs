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
		//ステージのX軸の最大値
		public static float stageMaxPositionY = 400.0f;
		//ステージのX軸の最小値
		public static float stageMinPositionY = 0.0f;
		//通常弾のダメージ量
		public static int normalBulletDamage = 1;
		//スナイパーライフルのダメージ量
		public static int sniperBulletDamage = 2;
		//ロケット弾のダメージ量
		public static int rocketBombDamage = 10;
		//武器アイテムの名称
		public static string weaponItemName = "WeaponItem";
		//通常銃アイテムの名称
		public static string normalGunItemName = "NormalGunItem";
		//ロケットランチャーアイテムの名称
		public static string rocketLauncherItemName = "RocketLauncherItem";
		//スナイパーライフルロケットランチャーアイテムの名称
		public static string sniperRifleItemName = "SniperRifleItem";
		//ショットガンアイテムの名称
		public static string shotGunItemName = "ShotGunItem";
		//通常弾の名称
		public static string normalBulletName = "Bullet";
		//ロケット弾の名称
		public static string rocketBombName = "RocketBomb";
		//スナイパーライフル弾の名称
		public static string sniperBulletName = "SniperBullet";
		//敵弾の名称
		public static string enemyBulletName = "EnemyBullet";
		//フィールドの名称
		public static string fieldName = "Field";
		//フィールドの名称
		public static string enemyName = "Enemy";
	}
}
