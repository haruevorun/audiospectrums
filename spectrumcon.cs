using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class spectrumcon : MonoBehaviour {
	//配列の長さ指定二の累乗でなければならない
	public int resolution = 1024;
	//オブジェクト指定
	public Transform lowlowMeter, lowhighMeter, midMeter, highlowMeter,highhighMeter;
	//スペクトラムの範囲指定
	public float lowlowFreqThreshold = 200,lowhighFreqThreshold = 300, midFreqThreshold = 400, highlowFreqThreshold = 500, highhighFreqThreshold = 600;
	//エンハンス指定
	public float lowlowEnhance = 1f, lowhighEnhance = 1f, midEnhance = 1f, highlowEnhance = 1f, highhighEnhance = 1f;

	private ParticleSystem particle;
	private AudioSource audio_;

	void Start()
	{
		//コンポーネント取得
		audio_ = GetComponent<AudioSource>();
		//曲再生
		audio_.Play();
	}

	void Update() {
		//オーディオソースで出力したデータをhamming関数でfftして出力している
		var spectrum = audio_.GetSpectrumData(resolution, 0, FFTWindow.Hamming);
		//オーディオのサンプルレートを指定した長さで割って頻度にしてると思ってる（謎）
		var deltaFreq = AudioSettings.outputSampleRate / resolution;
		float lowlow = 0f,lowhigh = 0f, mid = 0f,highlow = 0f, highhigh = 0f;
		//指定した配列の長さに対するスペクトラムデータを出力
		for (var i = 0; i < resolution; ++i) {
			var freq = deltaFreq * i;
			if      (freq <= lowlowFreqThreshold)  lowlow  += spectrum[i];
			else if (freq <= lowhighFreqThreshold)  lowhigh  += spectrum[i];
			else if (freq <= midFreqThreshold)  mid  += spectrum[i];
			else if (freq <= highlowFreqThreshold) highlow += spectrum[i];
			else if (freq <= highhighFreqThreshold) highhigh += spectrum[i];
		
		}
		//スペクトラムデータに各パラメータを調整するためにエンハンスしています
		lowlow  *= lowlowEnhance;
		lowhigh *= lowhighEnhance;
		mid  *= midEnhance;
		highlow *= highlowEnhance;
		highhigh *= highhighEnhance;
		//Y座標のローカルスケールを更新しています
		lowlowMeter.localScale  = new Vector3(lowlowMeter.localScale.x,  lowlow,  lowlowMeter.localScale.z);
		lowhighMeter.localScale = new Vector3 (lowhighMeter.localScale.x, lowhigh, lowhighMeter.localScale.z);
		midMeter.localScale  = new Vector3(midMeter.localScale.x,  mid ,  midMeter.localScale.z);
		highlowMeter.localScale = new Vector3 (highlowMeter.localScale.x, highlow , highlowMeter.localScale.z);
		highhighMeter.localScale = new Vector3(highhighMeter.localScale.x, highhigh , highhighMeter.localScale.z);
	}

}
