using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(VRTeleporter))]
public class TeleportSoundEvent : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string OnTeleportSound;
    [FMODUnity.EventRef]
    public string OnTeleportArchActiveSound;
    [FMODUnity.EventRef]
    public string OnTeleportArchHitsFloorSound;

    FMOD.Studio.EventInstance teleportEvent;
    FMOD.Studio.ParameterInstance DistanceToTarget;
    private Vector3 cameraPosition;
    private VRTeleporter teleport;

    FMOD.Studio.EventInstance arch;
    // Use this for initialization
    void Start()
    {
        teleport = GetComponent<VRTeleporter>();
        teleport.OnTeleportBegin += PlayTeleportSound;
        teleport.OnArchActive += PlayArchSound;
        teleport.OnArchHitFloor += PlayArchHitsFloorSound;

        teleportEvent = FMODUnity.RuntimeManager.CreateInstance(OnTeleportSound);
        teleportEvent.getParameter("DistanceToTarget", out DistanceToTarget);
        arch = FMODUnity.RuntimeManager.CreateInstance(OnTeleportArchActiveSound);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void PlayArchHitsFloorSound(GameObject hitpoint)
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached(OnTeleportArchHitsFloorSound, hitpoint);
    }

    public void PlayArchSound(bool active)
    {       
        
        if (active)
        {            
            //arch.setParameterValue("FullHealth", restoreAll ? 1.0f : 0.0f);
            arch.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
            arch.start();           
        }

        else
        {
            arch.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
    }

    public void PlayTeleportSound(float distanceToTarget)
    {
        cameraPosition = teleport.transform.position;
        float pitch;
        float finalPitch;
        FMOD.Studio.PLAYBACK_STATE state;
        teleportEvent.getPlaybackState(out state);
       
        teleportEvent.getPitch(out pitch, out finalPitch);       
        string EventSound = OnTeleportSound;      

        teleportEvent.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(cameraPosition));
        teleportEvent.setPitch(1 / distanceToTarget);
        teleportEvent.start();
        teleportEvent.getPlaybackState(out state);
        StartCoroutine(ResetPitch());       
       
    }

    IEnumerator ResetPitch()
    {
        float pitch;
        float finalPitch;
        FMOD.Studio.PLAYBACK_STATE state;
        teleportEvent.getPlaybackState(out state);
        while (state != FMOD.Studio.PLAYBACK_STATE.STOPPED)
        {
            teleportEvent.getPlaybackState(out state);
            yield return null;
        }
        teleportEvent.setPitch(5);
        teleportEvent.getPitch(out pitch, out finalPitch);        
    }
}
