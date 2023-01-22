using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util
{
	class Constants
    {
		//スクリーン横幅
		public static int screen_width = 1920;
		//スクリーン縦幅
		public static int screen_height = 1080;
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
		public static float stageMinPositionY = -50.0f;
		//通常弾のダメージ量
		public static int normalBulletDamage = 1;
		//通常銃の射程距離
		public static float normalBulletRange = 100;
		//スナイパーライフルのダメージ量
		public static int sniperBulletDamage = 2;
		//スナイパーライフルの射程距離
		public static float sniperBulletRange = 100;
		//ロケット弾のダメージ量
		public static int rocketBombDamage = 10;
		//ロケット弾の射程距離
		public static float rocketBombRange = 100;
		//通常銃の射程距離
		public static float shotGunBulletRange = 70;
		//マウスのY軸ポジションの名前
		public static FormattableString titleSceneName = $"TitleScene";
		//マウスのY軸ポジションの名前
		public static FormattableString gameSceneName = $"GameScene";
		//マウスのY軸ポジションの名前
		public static FormattableString endSceneName = $"EndScene";
		//武器アイテムの名称
		public static FormattableString weaponItemName = $"WeaponItem";
		//通常銃アイテムの名称
		public static FormattableString normalGunItemName = $"NormalGunItem";
		//ロケットランチャーアイテムの名称
		public static FormattableString rocketLauncherItemName = $"RocketLauncherItem";
		//スナイパーライフルロケットランチャーアイテムの名称
		public static FormattableString sniperRifleItemName = $"SniperRifleItem";
		//ショットガンアイテムの名称
		public static FormattableString shotGunItemName = $"ShotGunItem";
		//通常弾の名称
		public static FormattableString normalBulletName = $"Bullet";
		//ロケット弾の名称
		public static FormattableString rocketBombName = $"RocketBomb";
		//スナイパーライフル弾の名称
		public static FormattableString sniperBulletName = $"SniperBullet";
		//敵弾の名称
		public static FormattableString enemyBulletName = $"EnemyBullet";
		//フィールドの名称
		public static FormattableString fieldName = $"Field";
		//敵の名称
		public static FormattableString enemyName = $"Enemy";
		//敵の名称
		public static FormattableString ufoName = $"UFO";
		//右トリガーの名称
		public static FormattableString rTriggerName = $"R_Trigger";
		//左トリガーの名称
		public static FormattableString lTriggerName = $"L_Trigger";
		//左スティックの横方向の名称
		public static FormattableString lStickHorizontalName = $"L_Stick_H";
		//左スティックの縦方向の名称
		public static FormattableString lStickVerticalName = $"L_Stick_V";
		//右スティックの横方向の名称
		public static FormattableString rStickHorizontalName = $"R_Stick_H";
		//右スティックの縦方向の名称
		public static FormattableString rStickVerticalName = $"R_Stick_V";
		//マウスのY軸ポジションの名前
		public static FormattableString mouseAxisYName = $"Mouse Y";
		//マウスのY軸ポジションの名前
		public static FormattableString mouseAxisXName = $"Mouse X";
		//ステージに出現するエネミーの種類
		public static int normalEnemy = 0;
		public static int jumpEnemy = 1;
		public static int stepEnemy = 2;
		public static int stayEnemy = 3;
	}
}
