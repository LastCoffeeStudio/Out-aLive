using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.PostProcessing;

public enum VolumeShape
{
    BOX, SPHERE
}

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(SphereCollider))]


public class PostProcessVolume : MonoBehaviour
{
    private SphereCollider _sphereCollider;
    private BoxCollider _boxCollider;

    [Header("VolumeMode")]
    public VolumeShape ShapeOfVolume = VolumeShape.BOX;
    [Header("SphereVolumeSetting")]
    public float OuterSphereRadius=1;
    public float InnerSphereRadius;
    [Header("BoxVolumeSetting")]
    public Vector3 OuterBoxSize=Vector3.one;
    private Vector3 _outerBoxSize=Vector3.one;
  
    public float OuterBoxSizeMultiplier = 1;
    private float _outerBoxSizeMultiplier = 1;
    public Vector3 InnerBoxSize = Vector3.zero;
    private Vector3 _innerBoxSize = Vector3.zero;
    public float InnerBoxSizeMultiplier = 1;
    private float _innerBoxSizeMultiplier = 1;

    [Header("PostProcessing Effects")]
    //public FogModel fog = new FogModel();
    public AntialiasingModel antialiasing = new AntialiasingModel();
    public AmbientOcclusionModel ambientOcclusion = new AmbientOcclusionModel();
    public ScreenSpaceReflectionModel screenSpaceReflection = new ScreenSpaceReflectionModel();
    public DepthOfFieldModel depthOfField = new DepthOfFieldModel();
    public MotionBlurModel motionBlur = new MotionBlurModel();
    public EyeAdaptationModel eyeAdaptation = new EyeAdaptationModel();
    public BloomModel bloom = new BloomModel();
    public ColorGradingModel colorGrading = new ColorGradingModel();
    public UserLutModel userLut = new UserLutModel();
    public ChromaticAberrationModel chromaticAberration = new ChromaticAberrationModel();
    public GrainModel grain = new GrainModel();
    public VignetteModel vignette = new VignetteModel();
    public DitheringModel dithering = new DitheringModel();


    [Header("ResetAllValues")]
    public PostProcessingProfile _ResetProfile;


    private bool _hasJustStarted = true;
    // Use this for initialization
    void Start () {
        
        _sphereCollider = this.GetComponent<SphereCollider>();
        _sphereCollider.isTrigger = true;
        _boxCollider = this.GetComponent<BoxCollider>();
        _boxCollider.isTrigger = true;

        this.transform.localScale = Vector3.one;
        
       if (ShapeOfVolume == VolumeShape.BOX)
        {
            
            _boxCollider.enabled = true;
            _sphereCollider.enabled = false;

        }
        else if (ShapeOfVolume == VolumeShape.SPHERE)
        {
            
            _boxCollider.enabled = false;
            _sphereCollider.enabled = true;
        }
    }

    void OnValidate()
    {
        CheckColliderShape();
    }
	// Update is called once per frame
	void Update () {
		
	}


    void OnTriggerStay(Collider other)
    {
        PostProcessVolumeReceiver receivers = other.gameObject.GetComponent<PostProcessVolumeReceiver>();
        if (receivers!=null)
        {
            var thisVolume = this.GetComponent<PostProcessVolume>();
            receivers.SetValues(ref thisVolume, GradientPercentage(other.transform.position));
        }
    }

    void OnTriggerExit(Collider other)
    {
        PostProcessVolumeReceiver receivers = other.gameObject.GetComponent<PostProcessVolumeReceiver>();
        if (receivers != null)
        {
            receivers.ResetValues();
        }
    }


    float GradientPercentage(Vector3 position)
    {
        Vector3 distanceVec = this.transform.position - position;

        switch (ShapeOfVolume)
        {


                case VolumeShape.BOX:
                distanceVec = Quaternion.Inverse(this.transform.rotation)*distanceVec;
                float distanceXPerc = Mathf.Clamp01((Mathf.Abs(distanceVec.x) - InnerBoxSize.x * 0.5f) / ((_boxCollider.size.x - InnerBoxSize.x) * 0.5f));
                float distanceYPerc = Mathf.Clamp01((Mathf.Abs(distanceVec.y) - InnerBoxSize.y * 0.5f) / ((_boxCollider.size.y - InnerBoxSize.y) * 0.5f));
                float distanceZPerc = Mathf.Clamp01((Mathf.Abs(distanceVec.z) - InnerBoxSize.z * 0.5f) / ((_boxCollider.size.z - InnerBoxSize.z) * 0.5f));

                float max = Mathf.Max(distanceXPerc, distanceYPerc, distanceZPerc);
                return Mathf.Clamp01(1 - max);
                break;
                case VolumeShape.SPHERE:
                float percentage= (distanceVec.magnitude-InnerSphereRadius)/ (_sphereCollider.radius-InnerSphereRadius);
                return Mathf.Clamp01(1 - percentage);
                break;
        }

        return 0;
    }

    public void ResetValues()
    {
        if (_ResetProfile==null)
        {
            Debug.LogError("No ProfileEntered");
            return;
        }

        //Enable/disable elements
        antialiasing.enabled = _ResetProfile.antialiasing.enabled;
        ambientOcclusion.enabled = _ResetProfile.ambientOcclusion.enabled;
        screenSpaceReflection.enabled = _ResetProfile.screenSpaceReflection.enabled;
        depthOfField.enabled = _ResetProfile.depthOfField.enabled;
        motionBlur.enabled = _ResetProfile.motionBlur.enabled;
        eyeAdaptation.enabled = _ResetProfile.eyeAdaptation.enabled;
        bloom.enabled = _ResetProfile.bloom.enabled;
        colorGrading.enabled = _ResetProfile.colorGrading.enabled;
        userLut.enabled = _ResetProfile.userLut.enabled;
        chromaticAberration.enabled = _ResetProfile.chromaticAberration.enabled;
        grain.enabled = _ResetProfile.grain.enabled;
        vignette.enabled = _ResetProfile.vignette.enabled;
        dithering.enabled = _ResetProfile.dithering.enabled;

        //Copy settings
        antialiasing.settings = _ResetProfile.antialiasing.settings;
        ambientOcclusion.settings = _ResetProfile.ambientOcclusion.settings;
        screenSpaceReflection.settings = _ResetProfile.screenSpaceReflection.settings;
        depthOfField.settings = _ResetProfile.depthOfField.settings;
        motionBlur.settings = _ResetProfile.motionBlur.settings;
        eyeAdaptation.settings = _ResetProfile.eyeAdaptation.settings;
        bloom.settings = _ResetProfile.bloom.settings;
        colorGrading.settings = _ResetProfile.colorGrading.settings;
        userLut.settings = _ResetProfile.userLut.settings;
        chromaticAberration.settings = _ResetProfile.chromaticAberration.settings;
        grain.settings = _ResetProfile.grain.settings;
        vignette.settings = _ResetProfile.vignette.settings;
        dithering.settings = _ResetProfile.dithering.settings;

    }


    public void CheckColliderShape()
    {
        if (_hasJustStarted)
        {
            _hasJustStarted = false;
            _outerBoxSize = OuterBoxSize/ OuterBoxSizeMultiplier;
            _innerBoxSize = InnerBoxSize/InnerBoxSizeMultiplier;
        }

        this.transform.localScale = Vector3.one;
        if (_boxCollider==null|| _sphereCollider==null)
        {
            _sphereCollider = this.GetComponent<SphereCollider>();
            _sphereCollider.isTrigger = true;
            _boxCollider = this.GetComponent<BoxCollider>();
            _boxCollider.isTrigger = true;
        }

        OuterSphereRadius = Mathf.Clamp(OuterSphereRadius, 0, float.PositiveInfinity);
        _sphereCollider.radius = OuterSphereRadius;
        InnerSphereRadius = Mathf.Clamp(InnerSphereRadius, 0, _sphereCollider.radius);

        //OuterBoxSize
        if (_outerBoxSizeMultiplier!=OuterBoxSizeMultiplier)
        {
            OuterBoxSizeMultiplier = Mathf.Clamp(OuterBoxSizeMultiplier, 0.01f, float.PositiveInfinity);

            OuterBoxSize = _outerBoxSize*OuterBoxSizeMultiplier;
            _outerBoxSizeMultiplier = OuterBoxSizeMultiplier;
        }

        OuterBoxSize.x = Mathf.Clamp(OuterBoxSize.x, 0, float.PositiveInfinity);
        OuterBoxSize.y = Mathf.Clamp(OuterBoxSize.y, 0, float.PositiveInfinity);
        OuterBoxSize.z = Mathf.Clamp(OuterBoxSize.z, 0, float.PositiveInfinity);
        _outerBoxSize = OuterBoxSize/_outerBoxSizeMultiplier;

        _boxCollider.size = OuterBoxSize;

        //InnerBoxSize

        if (_innerBoxSizeMultiplier != InnerBoxSizeMultiplier)
        {
            InnerBoxSizeMultiplier = Mathf.Clamp(InnerBoxSizeMultiplier, 0.01f, float.PositiveInfinity);
            InnerBoxSize = _innerBoxSize * InnerBoxSizeMultiplier;
            _innerBoxSizeMultiplier = InnerBoxSizeMultiplier;
        }

        _innerBoxSize = InnerBoxSize / _innerBoxSizeMultiplier;

        InnerBoxSize.x = Mathf.Clamp(InnerBoxSize.x , 0, _boxCollider.size.x);
        InnerBoxSize.y = Mathf.Clamp(InnerBoxSize.y , 0, _boxCollider.size.y);
        InnerBoxSize.z = Mathf.Clamp(InnerBoxSize.z , 0, _boxCollider.size.z);
        //GeneralInnerScale = 1;


        if (ShapeOfVolume == VolumeShape.BOX )
        {
            //if (!_boxCollider.enabled)
            //{
            _boxCollider.enabled = true;
            _sphereCollider.enabled = false;
            //}

        }
        else if (ShapeOfVolume == VolumeShape.SPHERE )
        {
            //if (!_sphereCollider.enabled)
            //{
            _boxCollider.enabled = false;
            _sphereCollider.enabled = true;
            //}
        }
    }

    void OnDrawGizmos()
    {
        CheckColliderShape();
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        Gizmos.matrix = rotationMatrix;
        switch (ShapeOfVolume)
        {
                case VolumeShape.BOX:
                Gizmos.color = Color.blue;
                
                Gizmos.DrawWireCube(Vector3.zero, _boxCollider.size);
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireCube(Vector3.zero, InnerBoxSize);
                break;
                
                case VolumeShape.SPHERE:
                Gizmos.color=Color.blue;
                Gizmos.DrawWireSphere(Vector3.zero, _sphereCollider.radius);
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(Vector3.zero, InnerSphereRadius);

                break;
        }

        Gizmos.matrix=Matrix4x4.identity;
    }
}
