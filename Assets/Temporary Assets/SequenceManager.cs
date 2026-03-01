using UnityEngine;
using System.Collections;

public class SequenceManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject revealblock;

    public GameObject target_1;
    public Vector3 translation_point_1;

    public GameObject target_2;
    public Vector3 translation_point_2;

    public GameObject target_3;
    public Vector3 translation_point_3;

    public GameObject target_4;
    public Vector3 translation_point_4;

    public GameObject target_5;
    public Vector3 translation_point_5;

    public Vector3 translation_final;

    public float delay_initial_1 = 3f;
    public float delay_block = 2f;
    public float delay_between = .1f;

    public float delay_face = 5;

    private MovableCameraManager mcm;

    IEnumerator ActionSequence()
    {
        yield return new WaitForSeconds(delay_initial_1);

        Destroy(revealblock);

        yield return new WaitForSeconds(delay_block);

        mcm.SetFocus(target_1, translation_point_1, instant: true, relative: true);
        yield return new WaitForSeconds(delay_between);

        mcm.SetFocus(target_2, translation_point_2, instant: true, relative: true);
        yield return new WaitForSeconds(delay_between);

        mcm.SetFocus(target_3, translation_point_3, instant: true, relative: true);
        yield return new WaitForSeconds(delay_between);

        mcm.SetFocus(target_4, translation_point_4, instant: true, relative: true);
        yield return new WaitForSeconds(delay_between);

        mcm.SetFocus(target_5, translation_point_5, instant: true, relative: true);
        yield return new WaitForSeconds(delay_face);

        mcm.SetFocus(target_2, translation_final, instant: true, relative: true);

    }

    void Start()
    {
        mcm = Camera.main.transform.GetComponent<MovableCameraManager>();
        StartCoroutine(ActionSequence());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
