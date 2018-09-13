//Not Included: Bloom_LensDirt_Texture; Colorgrading GradingCurves; Fog; LUT Texture

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

[RequireComponent(typeof(PostProcessingBehaviour))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]

public class PostProcessVolumeReceiver : MonoBehaviour
{
    private PostProcessingProfile _profileOriginal;
    private PostProcessingProfile _profileCopy;

    //private FogModel.Settings fogSettings;
    private AntialiasingModel.Settings antialiasingSettings;
    private AmbientOcclusionModel.Settings ambientOcclusionSettings;
   private ScreenSpaceReflectionModel.Settings screenSpaceReflectionSettings;
   private DepthOfFieldModel.Settings depthOfFieldSettings;
   private MotionBlurModel.Settings motionBlurSettings;
   private EyeAdaptationModel.Settings eyeAdaptationSettings;
   private BloomModel.Settings bloomSettings;
   private ColorGradingModel.Settings colorGradingSettings;
   private UserLutModel.Settings userLutSettings;
   private ChromaticAberrationModel.Settings chromaticAberrationSettings;
   private GrainModel.Settings grainSettings;
   private VignetteModel.Settings vignetteSettings;
   private DitheringModel.Settings ditheringSettings;
    // Use this for initialization
    void Start ()
    {
        _profileOriginal=this.GetComponent<PostProcessingBehaviour>().profile;
        if (_profileOriginal==null)
        {
            Debug.LogError("No Post ProcessingProfile added to camera!");
            return;
        }
        _profileCopy = Instantiate(_profileOriginal);
        this.GetComponent<PostProcessingBehaviour>().profile = _profileCopy;

        colorGradingSettings = _profileCopy.colorGrading.settings;
        this.GetComponent<Rigidbody>().isKinematic = false;
        this.GetComponent<Rigidbody>().useGravity = false;
        this.GetComponent<Collider>().isTrigger=true;
    }
	
	// Update is called once per frame
    public void SetValues(ref PostProcessVolume volume, float percentage)
    {
        if (volume.antialiasing.enabled )
        {
            _profileCopy.antialiasing.enabled = true;
            //Give all values in case something changes/was forgotten
            _profileCopy.antialiasing.settings = volume.antialiasing.settings;


            antialiasingSettings.method = volume.antialiasing.settings.method;
            if (volume.antialiasing.settings.method == AntialiasingModel.Method.Taa)
            {


                antialiasingSettings.taaSettings.jitterSpread =
                    Mathf.Lerp(_profileOriginal.antialiasing.settings.taaSettings.jitterSpread,
                        volume.antialiasing.settings.taaSettings.jitterSpread, percentage);
                antialiasingSettings.taaSettings.stationaryBlending =
                    Mathf.Lerp(_profileOriginal.antialiasing.settings.taaSettings.stationaryBlending,
                        volume.antialiasing.settings.taaSettings.stationaryBlending, percentage);
                antialiasingSettings.taaSettings.motionBlending =
                    Mathf.Lerp(_profileOriginal.antialiasing.settings.taaSettings.motionBlending,
                        volume.antialiasing.settings.taaSettings.motionBlending, percentage);
                antialiasingSettings.taaSettings.sharpen =
                    Mathf.Lerp(_profileOriginal.antialiasing.settings.taaSettings.sharpen,
                        volume.antialiasing.settings.taaSettings.sharpen, percentage);
                _profileCopy.antialiasing.settings = antialiasingSettings;
            }
            else if (volume.antialiasing.settings.method == AntialiasingModel.Method.Fxaa)
            {
                antialiasingSettings.fxaaSettings.preset = volume.antialiasing.settings.fxaaSettings.preset;
                _profileCopy.antialiasing.settings = antialiasingSettings;
            }
        }

        if (volume.ambientOcclusion.enabled)
        {
            _profileCopy.ambientOcclusion.enabled = true;
            //Give all values in case something changes/was forgotten
            _profileCopy.ambientOcclusion.settings = volume.ambientOcclusion.settings;


            ambientOcclusionSettings.intensity=Mathf.Lerp(_profileOriginal.ambientOcclusion.settings.intensity, volume.ambientOcclusion.settings.intensity,percentage);
            ambientOcclusionSettings.radius=Mathf.Lerp(_profileOriginal.ambientOcclusion.settings.radius, volume.ambientOcclusion.settings.radius,percentage);
            ambientOcclusionSettings.sampleCount = volume.ambientOcclusion.settings.sampleCount;
            ambientOcclusionSettings.downsampling = volume.ambientOcclusion.settings.downsampling;
            ambientOcclusionSettings.forceForwardCompatibility = volume.ambientOcclusion.settings.forceForwardCompatibility;
            ambientOcclusionSettings.highPrecision = volume.ambientOcclusion.settings.highPrecision;
            ambientOcclusionSettings.ambientOnly = volume.ambientOcclusion.settings.ambientOnly;
            _profileCopy.ambientOcclusion.settings = ambientOcclusionSettings;
        }
        else
        {
            _profileCopy.ambientOcclusion.enabled = false;

        }

        if (volume.screenSpaceReflection.enabled)
        {
            _profileCopy.screenSpaceReflection.enabled = true;
            //Give all values in case something changes/was forgotten
            _profileCopy.screenSpaceReflection.settings = volume.screenSpaceReflection.settings;


            //Reflection
            screenSpaceReflectionSettings.reflection.blendType =volume.screenSpaceReflection.settings.reflection.blendType;
            screenSpaceReflectionSettings.reflection.reflectionQuality = volume.screenSpaceReflection.settings.reflection.reflectionQuality;
            screenSpaceReflectionSettings.reflection.maxDistance =Mathf.Lerp(_profileOriginal.screenSpaceReflection.settings.reflection.maxDistance, volume.screenSpaceReflection.settings.reflection.maxDistance, percentage);
            screenSpaceReflectionSettings.reflection.iterationCount =(int)Mathf.Lerp(_profileOriginal.screenSpaceReflection.settings.reflection.iterationCount, volume.screenSpaceReflection.settings.reflection.iterationCount, percentage);
            screenSpaceReflectionSettings.reflection.stepSize =(int)Mathf.Lerp(_profileOriginal.screenSpaceReflection.settings.reflection.stepSize, volume.screenSpaceReflection.settings.reflection.stepSize, percentage);
            screenSpaceReflectionSettings.reflection.widthModifier =Mathf.Lerp(_profileOriginal.screenSpaceReflection.settings.reflection.widthModifier, volume.screenSpaceReflection.settings.reflection.widthModifier, percentage);
            screenSpaceReflectionSettings.reflection.reflectionBlur = Mathf.Lerp(_profileOriginal.screenSpaceReflection.settings.reflection.reflectionBlur, volume.screenSpaceReflection.settings.reflection.reflectionBlur, percentage);
            screenSpaceReflectionSettings.reflection.reflectBackfaces = volume.screenSpaceReflection.settings.reflection.reflectBackfaces;

            //Intensity
            screenSpaceReflectionSettings.intensity.reflectionMultiplier = Mathf.Lerp(_profileOriginal.screenSpaceReflection.settings.intensity.reflectionMultiplier, volume.screenSpaceReflection.settings.intensity.reflectionMultiplier, percentage);
            screenSpaceReflectionSettings.intensity.fadeDistance = Mathf.Lerp(_profileOriginal.screenSpaceReflection.settings.intensity.fadeDistance, volume.screenSpaceReflection.settings.intensity.fadeDistance, percentage);
            screenSpaceReflectionSettings.intensity.fresnelFade = Mathf.Lerp(_profileOriginal.screenSpaceReflection.settings.intensity.fresnelFade, volume.screenSpaceReflection.settings.intensity.fresnelFade, percentage);
            screenSpaceReflectionSettings.intensity.fresnelFadePower = Mathf.Lerp(_profileOriginal.screenSpaceReflection.settings.intensity.fresnelFadePower, volume.screenSpaceReflection.settings.intensity.fresnelFadePower, percentage);

            //ScreenEdgeMask
            screenSpaceReflectionSettings.screenEdgeMask.intensity = Mathf.Lerp(_profileOriginal.screenSpaceReflection.settings.screenEdgeMask.intensity, volume.screenSpaceReflection.settings.screenEdgeMask.intensity, percentage);

            _profileCopy.screenSpaceReflection.settings = screenSpaceReflectionSettings;
        }
        else
        {
            _profileCopy.screenSpaceReflection.enabled = false;

        }

        if (volume.depthOfField.enabled)
        {
            _profileCopy.depthOfField.enabled = true;
            //Give all values in case something changes/was forgotten
            _profileCopy.depthOfField.settings = volume.depthOfField.settings;


            depthOfFieldSettings.focusDistance = Mathf.Lerp(_profileOriginal.depthOfField.settings.focusDistance, volume.depthOfField.settings.focusDistance, percentage);
            depthOfFieldSettings.aperture = Mathf.Lerp(_profileOriginal.depthOfField.settings.aperture, volume.depthOfField.settings.aperture, percentage);
            depthOfFieldSettings.useCameraFov = volume.depthOfField.settings.useCameraFov;
            depthOfFieldSettings.focalLength = Mathf.Lerp(_profileOriginal.depthOfField.settings.focalLength, volume.depthOfField.settings.focalLength, percentage);
            depthOfFieldSettings.kernelSize = volume.depthOfField.settings.kernelSize;

            _profileCopy.depthOfField.settings = depthOfFieldSettings;
        }
        else
        {
            _profileCopy.depthOfField.enabled = false;

        }

        if (volume.motionBlur.enabled)
        {
            _profileCopy.motionBlur.enabled = true;
            //Give all values in case something changes/was forgotten
            _profileCopy.motionBlur.settings = volume.motionBlur.settings;


            motionBlurSettings.shutterAngle=Mathf.Lerp(_profileOriginal.motionBlur.settings.shutterAngle, volume.motionBlur.settings.shutterAngle, percentage);
            motionBlurSettings.sampleCount=(int)Mathf.Lerp(_profileOriginal.motionBlur.settings.sampleCount, volume.motionBlur.settings.sampleCount, percentage);
            motionBlurSettings.frameBlending = Mathf.Lerp(_profileOriginal.motionBlur.settings.frameBlending, volume.motionBlur.settings.frameBlending, percentage);

            _profileCopy.motionBlur.settings = motionBlurSettings;
        }
        else
        {
            _profileCopy.motionBlur.enabled = false;

        }

        if (volume.eyeAdaptation.enabled)
        {
            _profileCopy.eyeAdaptation.enabled = true;
            //Give all values in case something changes/was forgotten
            _profileCopy.eyeAdaptation.settings = volume.eyeAdaptation.settings;


            //LuminosityRange
            eyeAdaptationSettings.logMin = (int)Mathf.Lerp(_profileOriginal.eyeAdaptation.settings.logMin, volume.eyeAdaptation.settings.logMin, percentage);
            eyeAdaptationSettings.logMax = (int) Mathf.Lerp(_profileOriginal.eyeAdaptation.settings.logMax, volume.eyeAdaptation.settings.logMax, percentage);

            //AutoExposure
            eyeAdaptationSettings.lowPercent = Mathf.Lerp(_profileOriginal.eyeAdaptation.settings.lowPercent, volume.eyeAdaptation.settings.lowPercent, percentage);
            eyeAdaptationSettings.highPercent = Mathf.Lerp(_profileOriginal.eyeAdaptation.settings.highPercent, volume.eyeAdaptation.settings.highPercent, percentage);

            eyeAdaptationSettings.minLuminance = Mathf.Lerp(_profileOriginal.eyeAdaptation.settings.minLuminance, volume.eyeAdaptation.settings.minLuminance, percentage);
            eyeAdaptationSettings.maxLuminance = Mathf.Lerp(_profileOriginal.eyeAdaptation.settings.maxLuminance, volume.eyeAdaptation.settings.maxLuminance, percentage);
            eyeAdaptationSettings.dynamicKeyValue = volume.eyeAdaptation.settings.dynamicKeyValue;
            eyeAdaptationSettings.keyValue = Mathf.Lerp(_profileOriginal.eyeAdaptation.settings.keyValue, volume.eyeAdaptation.settings.keyValue, percentage);

            //Adaptation
            eyeAdaptationSettings.adaptationType = volume.eyeAdaptation.settings.adaptationType;
            eyeAdaptationSettings.speedUp = Mathf.Lerp(_profileOriginal.eyeAdaptation.settings.speedUp, volume.eyeAdaptation.settings.speedUp, percentage);
            eyeAdaptationSettings.speedDown = Mathf.Lerp(_profileOriginal.eyeAdaptation.settings.speedDown, volume.eyeAdaptation.settings.speedDown, percentage);

            _profileCopy.eyeAdaptation.settings = eyeAdaptationSettings;
        }
        else
        {
            _profileCopy.eyeAdaptation.enabled = false;

        }

        if (volume.bloom.enabled)
        {
            _profileCopy.bloom.enabled = true;
            //Give all values in case something changes/was forgotten
            _profileCopy.bloom.settings = volume.bloom.settings;


            //Bloom
            bloomSettings.bloom.intensity=Mathf.Lerp(_profileOriginal.bloom.settings.bloom.intensity, volume.bloom.settings.bloom.intensity, percentage);
            bloomSettings.bloom.threshold=Mathf.Lerp(_profileOriginal.bloom.settings.bloom.threshold, volume.bloom.settings.bloom.threshold, percentage);
            bloomSettings.bloom.thresholdLinear=Mathf.Lerp(_profileOriginal.bloom.settings.bloom.thresholdLinear, volume.bloom.settings.bloom.thresholdLinear, percentage);
            bloomSettings.bloom.softKnee=Mathf.Lerp(_profileOriginal.bloom.settings.bloom.softKnee, volume.bloom.settings.bloom.softKnee, percentage);
            bloomSettings.bloom.radius=Mathf.Lerp(_profileOriginal.bloom.settings.bloom.radius, volume.bloom.settings.bloom.radius, percentage);
            bloomSettings.bloom.antiFlicker=volume.bloom.settings.bloom.antiFlicker;

            //Dirt
            bloomSettings.lensDirt.texture = _profileOriginal.bloom.settings.lensDirt.texture;
            bloomSettings.lensDirt.intensity = Mathf.Lerp(_profileOriginal.bloom.settings.lensDirt.intensity, volume.bloom.settings.lensDirt.intensity, percentage);

            _profileCopy.bloom.settings = bloomSettings;
        }
        else
        {
            _profileCopy.bloom.enabled = false;

        }

        if (volume.colorGrading.enabled)
        {
            _profileCopy.colorGrading.enabled = true;
            //Give all values in case something changes/was forgotten
            _profileCopy.colorGrading.settings = volume.colorGrading.settings;


            //ToneMapping
            colorGradingSettings.tonemapping.tonemapper = volume.colorGrading.settings.tonemapping.tonemapper;
            colorGradingSettings.tonemapping.neutralBlackIn =
                Mathf.Lerp(_profileOriginal.colorGrading.settings.tonemapping.neutralBlackIn,
                    volume.colorGrading.settings.tonemapping.neutralBlackIn, percentage);
            colorGradingSettings.tonemapping.neutralWhiteIn =
                Mathf.Lerp(_profileOriginal.colorGrading.settings.tonemapping.neutralWhiteIn,
                    volume.colorGrading.settings.tonemapping.neutralWhiteIn, percentage);
            colorGradingSettings.tonemapping.neutralBlackOut =
                Mathf.Lerp(_profileOriginal.colorGrading.settings.tonemapping.neutralBlackOut,
                    volume.colorGrading.settings.tonemapping.neutralBlackOut, percentage);
            colorGradingSettings.tonemapping.neutralWhiteOut =
                Mathf.Lerp(_profileOriginal.colorGrading.settings.tonemapping.neutralWhiteOut,
                    volume.colorGrading.settings.tonemapping.neutralWhiteOut, percentage);
            colorGradingSettings.tonemapping.neutralWhiteLevel =
                Mathf.Lerp(_profileOriginal.colorGrading.settings.tonemapping.neutralWhiteLevel,
                    volume.colorGrading.settings.tonemapping.neutralWhiteLevel, percentage);
            colorGradingSettings.tonemapping.neutralWhiteClip =
                Mathf.Lerp(_profileOriginal.colorGrading.settings.tonemapping.neutralWhiteClip,
                    volume.colorGrading.settings.tonemapping.neutralWhiteClip, percentage);


            //basic
            colorGradingSettings.basic.temperature = Mathf.Lerp(
                _profileOriginal.colorGrading.settings.basic.temperature, volume.colorGrading.settings.basic.temperature,
                percentage);
            colorGradingSettings.basic.hueShift = Mathf.Lerp(_profileOriginal.colorGrading.settings.basic.hueShift,
                volume.colorGrading.settings.basic.hueShift, percentage);
            colorGradingSettings.basic.contrast = Mathf.Lerp(_profileOriginal.colorGrading.settings.basic.contrast,
                volume.colorGrading.settings.basic.contrast, percentage);
            colorGradingSettings.basic.postExposure =
                Mathf.Lerp(_profileOriginal.colorGrading.settings.basic.postExposure,
                    volume.colorGrading.settings.basic.postExposure, percentage);
            colorGradingSettings.basic.saturation = Mathf.Lerp(_profileOriginal.colorGrading.settings.basic.saturation,
                volume.colorGrading.settings.basic.saturation, percentage);
            colorGradingSettings.basic.tint = Mathf.Lerp(_profileOriginal.colorGrading.settings.basic.tint,
                volume.colorGrading.settings.basic.tint, percentage);

            //channelmixer

            colorGradingSettings.channelMixer.currentEditingChannel =
                volume.colorGrading.settings.channelMixer.currentEditingChannel;
            colorGradingSettings.channelMixer.red = Vector3.Lerp(
                _profileOriginal.colorGrading.settings.channelMixer.red, volume.colorGrading.settings.channelMixer.red,
                percentage);
            colorGradingSettings.channelMixer.green =
                Vector3.Lerp(_profileOriginal.colorGrading.settings.channelMixer.green,
                    volume.colorGrading.settings.channelMixer.green, percentage);
            colorGradingSettings.channelMixer.blue =
                Vector3.Lerp(_profileOriginal.colorGrading.settings.channelMixer.blue,
                    volume.colorGrading.settings.channelMixer.blue, percentage);

            //Trackballs
            colorGradingSettings.colorWheels.mode = volume.colorGrading.settings.colorWheels.mode;
            if (colorGradingSettings.colorWheels.mode == ColorGradingModel.ColorWheelMode.Linear)
            {
                colorGradingSettings.colorWheels.linear.gain =
                    Color.Lerp(_profileOriginal.colorGrading.settings.colorWheels.linear.gain,
                        volume.colorGrading.settings.colorWheels.linear.gain, percentage);
                colorGradingSettings.colorWheels.linear.gamma =
                    Color.Lerp(_profileOriginal.colorGrading.settings.colorWheels.linear.gamma,
                        volume.colorGrading.settings.colorWheels.linear.gamma, percentage);
                colorGradingSettings.colorWheels.linear.lift =
                    Color.Lerp(_profileOriginal.colorGrading.settings.colorWheels.linear.lift,
                        volume.colorGrading.settings.colorWheels.linear.lift, percentage);


            }
            else if (colorGradingSettings.colorWheels.mode == ColorGradingModel.ColorWheelMode.Log)
            {
                colorGradingSettings.colorWheels.log.slope =
                    Color.Lerp(_profileOriginal.colorGrading.settings.colorWheels.log.slope,
                        volume.colorGrading.settings.colorWheels.log.slope, percentage);
                colorGradingSettings.colorWheels.log.power =
                    Color.Lerp(_profileOriginal.colorGrading.settings.colorWheels.log.power,
                        volume.colorGrading.settings.colorWheels.log.power, percentage);
                colorGradingSettings.colorWheels.log.offset =
                    Color.Lerp(_profileOriginal.colorGrading.settings.colorWheels.log.offset,
                        volume.colorGrading.settings.colorWheels.log.offset, percentage);

            }

            colorGradingSettings.curves = _profileOriginal.colorGrading.settings.curves;

            _profileCopy.colorGrading.settings = colorGradingSettings;
        }
        else
        {
            _profileCopy.colorGrading.enabled = false;

        }
        // LUT
        if (volume.userLut.enabled)
        {
            _profileCopy.userLut.enabled = true;
            //Give all values in case something changes/was forgotten
            _profileCopy.userLut.settings = volume.userLut.settings;


            userLutSettings.lut = _profileOriginal.userLut.settings.lut;
            userLutSettings.contribution = Mathf.Lerp(_profileOriginal.userLut.settings.contribution, volume.userLut.settings.contribution, percentage);
        
            _profileCopy.userLut.settings = userLutSettings;
        }
        else
        {
            _profileCopy.userLut.enabled = false;

        }

        //Chromatic aberration
        if (volume.chromaticAberration.enabled)
        {
            _profileCopy.chromaticAberration.enabled = true;
            //Give all values in case something changes/was forgotten
            _profileCopy.chromaticAberration.settings = volume.chromaticAberration.settings;

            chromaticAberrationSettings.spectralTexture = volume.chromaticAberration.settings.spectralTexture;
            chromaticAberrationSettings.intensity = Mathf.Lerp(_profileOriginal.chromaticAberration.settings.intensity, volume.chromaticAberration.settings.intensity, percentage);
            _profileCopy.chromaticAberration.settings = chromaticAberrationSettings;
        }
        else
        {
            _profileCopy.chromaticAberration.enabled = false;

        }

        //Grain
        if (volume.grain.enabled)
        {
            _profileCopy.grain.enabled = true;
            //Give all values in case something changes/was forgotten
            _profileCopy.grain.settings = volume.grain.settings;

            grainSettings.intensity = Mathf.Lerp(_profileOriginal.grain.settings.intensity, volume.grain.settings.intensity, percentage);
            grainSettings.luminanceContribution = Mathf.Lerp(_profileOriginal.grain.settings.luminanceContribution, volume.grain.settings.luminanceContribution, percentage);
            grainSettings.size = Mathf.Lerp(_profileOriginal.grain.settings.size, volume.grain.settings.size, percentage);
            grainSettings.colored = volume.grain.settings.colored;

            _profileCopy.grain.settings = grainSettings;
        }
        else
        {
            _profileCopy.grain.enabled = false;

        }

        //Vignette
        if (volume.vignette.enabled)
        {
            _profileCopy.vignette.enabled = true;
            //Give all values in case something changes/was forgotten
            _profileCopy.vignette.settings = volume.vignette.settings;

            vignetteSettings.mode = volume.vignette.settings.mode;
            vignetteSettings.color = Color.Lerp(_profileOriginal.vignette.settings.color, volume.vignette.settings.color, percentage);

            if (vignetteSettings.mode==VignetteModel.Mode.Masked)
            {
                vignetteSettings.opacity = Mathf.Lerp(_profileOriginal.vignette.settings.opacity, volume.vignette.settings.opacity, percentage);

            }else if (vignetteSettings.mode == VignetteModel.Mode.Classic)
            {
                vignetteSettings.center = Vector3.Lerp(_profileOriginal.vignette.settings.center, volume.vignette.settings.center, percentage);

                vignetteSettings.intensity = Mathf.Lerp(_profileOriginal.vignette.settings.intensity, volume.vignette.settings.intensity, percentage);
                vignetteSettings.smoothness = Mathf.Lerp(_profileOriginal.vignette.settings.smoothness, volume.vignette.settings.smoothness, percentage);
                vignetteSettings.roundness = Mathf.Lerp(_profileOriginal.vignette.settings.roundness, volume.vignette.settings.roundness, percentage);
                vignetteSettings.rounded = volume.vignette.settings.rounded;
            }

            _profileCopy.vignette.settings = vignetteSettings;
        }
        else
        {
            _profileCopy.vignette.enabled = false;

        }

        if (volume.dithering.enabled)
        {

            _profileCopy.dithering.enabled = true;
        }
        else
        {
            _profileCopy.dithering.enabled = false;

        }


    }

    public void ResetValues()
    {
        //antialiasing
        if (_profileOriginal.antialiasing.enabled)
        {
            _profileCopy.antialiasing.enabled = true;
            _profileCopy.antialiasing.settings = _profileOriginal.antialiasing.settings;
        }
        else
        {
            _profileCopy.antialiasing.enabled = false;
        }

        //ambientOcclusion
        if (_profileOriginal.ambientOcclusion.enabled)
        {
            _profileCopy.ambientOcclusion.enabled = true;
            _profileCopy.ambientOcclusion.settings = _profileOriginal.ambientOcclusion.settings;
        }
        else
        {
            _profileCopy.ambientOcclusion.enabled = false;
        }

        //screenSpaceReflection
        if (_profileOriginal.screenSpaceReflection.enabled)
        {
            _profileCopy.screenSpaceReflection.enabled = true;
            _profileCopy.screenSpaceReflection.settings = _profileOriginal.screenSpaceReflection.settings;
        }
        else
        {
            _profileCopy.screenSpaceReflection.enabled = false;
        }

        //depthOfField
        if (_profileOriginal.depthOfField.enabled)
        {
            _profileCopy.depthOfField.enabled = true;
            _profileCopy.depthOfField.settings = _profileOriginal.depthOfField.settings;
        }
        else
        {
            _profileCopy.depthOfField.enabled = false;
        }

        //motionBlur
        if (_profileOriginal.motionBlur.enabled)
        {
            _profileCopy.motionBlur.enabled = true;
            _profileCopy.motionBlur.settings = _profileOriginal.motionBlur.settings;
        }
        else
        {
            _profileCopy.motionBlur.enabled = false;
        }

        //eyeAdaptation
        if (_profileOriginal.eyeAdaptation.enabled)
        {
            _profileCopy.eyeAdaptation.enabled = true;
            _profileCopy.eyeAdaptation.settings = _profileOriginal.eyeAdaptation.settings;
        }
        else
        {
            _profileCopy.eyeAdaptation.enabled = false;
        }

        //bloom
        if (_profileOriginal.bloom.enabled)
        {
            _profileCopy.bloom.enabled = true;
            _profileCopy.bloom.settings = _profileOriginal.bloom.settings;
        }
        else
        {
            _profileCopy.bloom.enabled = false;
        }
        
        //colorGrading
        if (_profileOriginal.colorGrading.enabled)
        {
            _profileCopy.colorGrading.enabled = true;
            _profileCopy.colorGrading.settings = _profileOriginal.colorGrading.settings;
        }
        else
        {
            _profileCopy.colorGrading.enabled = false;
        }

        //userLut
        if (_profileOriginal.userLut.enabled)
        {
            _profileCopy.userLut.enabled = true;
            _profileCopy.userLut.settings = _profileOriginal.userLut.settings;
        }
        else
        {
            _profileCopy.userLut.enabled = false;
        }

        //chromaticAberration
        if (_profileOriginal.chromaticAberration.enabled)
        {
            _profileCopy.chromaticAberration.enabled = true;
            _profileCopy.chromaticAberration.settings = _profileOriginal.chromaticAberration.settings;
        }
        else
        {
            _profileCopy.chromaticAberration.enabled = false;
        }

        //grain
        if (_profileOriginal.grain.enabled)
        {
            _profileCopy.grain.enabled = true;
            _profileCopy.grain.settings = _profileOriginal.grain.settings;
        }
        else
        {
            _profileCopy.grain.enabled = false;
        }

        //vignette
        if (_profileOriginal.vignette.enabled)
        {
            _profileCopy.vignette.enabled = true;
            _profileCopy.vignette.settings = _profileOriginal.vignette.settings;
        }
        else
        {
            _profileCopy.vignette.enabled = false;
        }

        //dithering
        if (_profileOriginal.dithering.enabled)
        {
            _profileCopy.dithering.enabled = true;
            _profileCopy.dithering.settings = _profileOriginal.dithering.settings;
        }
        else
        {
            _profileCopy.dithering.enabled = false;
        }
    }

}
