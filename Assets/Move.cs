using UnityEngine;

public class Move : MonoBehaviour {

    public GameObject goal;
    public float speed = 5f;
    Vector3 direction;

    void Start() {

        //direction = goal.transform.position - this.transform.position;
        //Translate(direction);
        //this.transform.Translate(direction);
        //this.transform.Translate(goal.transform.position);
    }

    private void LateUpdate() {
        direction = goal.transform.position - this.transform.position;
        this.transform.LookAt(goal.transform.position);
        Debug.DrawRay(this.transform.position, goal.transform.position, Color.red, 1);
        if (direction.magnitude > 2)
        {
            Vector3 velocity = direction.normalized * speed * Time.deltaTime;
            Translate(velocity);
            //this.transform.position = this.transform.position + velocity;
            //this.transform.Translate(velocity);
        }
    }

    void Translate(Vector3 direction)
    {
        this.transform.position = this.transform.position + direction;
    }

}
