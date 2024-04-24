using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;
using UnityEngine.Audio;
using System.Threading.Tasks;
using System.Threading;

public class FireShell : MonoBehaviour {

    public GameObject bullet;
    public GameObject turret;
    public GameObject enemy;
    public Transform turretBase;
    public float speed = 15.0f;
    public float rotSpeed = 1.5f;
    public float moveSpead = 1f;
    public int shotCoolDownTicks = 300;
    bool isShot = false;

    void CreateBullet() {

        GameObject shell = Instantiate(bullet, turret.transform.position, turret.transform.rotation);
        shell.GetComponent<Rigidbody>().velocity = speed * turretBase.forward;
    }

    float? RotateTurret()
    {
        float? angle = CalcAngle(true);
        if (angle != null)
        {
            turretBase.localEulerAngles = new Vector3(360f - (float)angle, 0f, 0f);
        }
        return angle;
    }

    float? CalcAngle(bool low)
    {
        Vector3 target = enemy.transform.position - this.transform.position;
        float y = target.y;
        target.y = 0f;
        float x = target.magnitude - 1;
        float gravity = 9.8f;
        float sSqr = speed * speed;
        float underSqrt = (sSqr * sSqr) - gravity * ((gravity * x * x) + (2 * y * sSqr));

        if (underSqrt >= 0f)
        {
            float root = Mathf.Sqrt(underSqrt);
            float highAngle = sSqr + root;
            float lowAngle = sSqr - root;
             
            if (low)
                return (Mathf.Atan2(lowAngle, gravity * x) * Mathf.Rad2Deg);
            else
                return (Mathf.Atan2(highAngle,gravity*x)* Mathf.Rad2Deg);
        }
        else
            return null;
    }

    void Update() 
    {
        Vector3 direction = (enemy.transform.position - this.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, lookRotation, Time.deltaTime * rotSpeed);
        float? angle = RotateTurret();
        Thread thread = new Thread(() => ShotTimer());

        if (angle != null)
        {
            if (!isShot)
            {
                isShot = true;
                CreateBullet();
                thread.Start();
            }
        }
        else
        {
            this.transform.Translate(0, 0, Time.deltaTime * moveSpead);
        }
    }

    void ShotTimer()
    {
        Thread.Sleep(shotCoolDownTicks);
        isShot = false;
    }

    Vector3 CalculateTrajectory() {

        Vector3 p = enemy.transform.position - this.transform.position;
        Vector3 v = enemy.transform.forward * enemy.GetComponent<Drive>().speed;
        float s = bullet.GetComponent<MoveShell>().speed;

        float a = Vector3.Dot(v, v) - s * s;
        float b = Vector3.Dot(p, v);
        float c = Vector3.Dot(p, p);
        float d = b * b - a * c;

        if (d < 0.1f) return Vector3.zero;

        float sqrt = Mathf.Sqrt(d);
        float t1 = (-b - sqrt) / c;
        float t2 = (-b + sqrt) / c;

        float t = 0.0f;
        if (t1 < 0.0f && t2 < 0.0f) return Vector3.zero;
        else if (t1 < 0.0f) t = t2;
        else if (t2 < 0.0f) t = t1;
        else {

            t = Mathf.Max(new float[] { t1, t2 });
        }
        return t * p + v;
    }
}
