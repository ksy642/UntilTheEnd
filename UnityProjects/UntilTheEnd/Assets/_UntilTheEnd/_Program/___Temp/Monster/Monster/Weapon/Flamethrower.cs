using UnityEngine;

namespace UntilTheEnd
{
    public class Flamethrower : Weapon
    {
        [Header("화염방사기")]
        public ParticleSystem fireParticles;  // 불꽃 파티클
        public float damagePerSecond = 1f;  // 초당 피해량

        private void Start()
        {
            weaponData.weaponName = "Flamethrower";
            weaponData.damage = damagePerSecond;
        }

        public override void WeaponChange()
        {
            base.WeaponChange();

            Debug.Log("화염방사기로 변경합니다,");



        }


        private void OnParticleCollision(GameObject other)
        {
            if (other.CompareTag("Orc"))
            {
                Orc orc = other.GetComponent<Orc>();
                if (orc != null)
                {
                    orc.TakeDamage(damagePerSecond);// * Time.deltaTime);
                }
            }
            else if (other.CompareTag("Goblin"))
            {
                Goblin gobline = other.GetComponent<Goblin>();
                if (gobline != null)
                {
                    gobline.TakeDamage(damagePerSecond);// * Time.deltaTime);
                }
            }
        }



        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                if (!fireParticles.isPlaying)
                    fireParticles.Play();
            }
            else
            {
                if (fireParticles.isPlaying)
                    fireParticles.Stop();
            }
        }

    }
}