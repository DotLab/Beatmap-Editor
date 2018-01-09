/*
 * Copyright (c) 2015 Allan Pichardo
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *  http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class AudioProcessor : MonoBehaviour {
	private double[] data;
	private double bpm;
	private double sampleRate = 44100;
	private double trackLength = 0;

	public double getBPM() {
		return bpm;
	}

	public void Detect(double sampleRate, double[] data) {
		this.sampleRate = sampleRate;
		this.data = data;

		Detect();
	}

	private void Detect() {
		trackLength = (float)data.Length / sampleRate;

		// 0.1s window ... 0.1*44100 = 4410 samples, lets adjust this to 3600 
		const int sampleStep = 3600;

		// calculate energy over windows of size sampleSetep
		List<double> energies = new List<double>();
		for (int i = 0; i < data.Length - sampleStep - 1; i += sampleStep) {
			energies.Add(rangeQuadSum(data, i, i + sampleStep));
		}

		int beats = 0;
		double average = 0;
		double sumOfSquaresOfDifferences = 0;
		double variance = 0;
		double newC = 0;
		List<double> variances = new List<double>();

		// how many energies before and after index for local energy average
		int offset = 10;

		for (int i = offset; i <= energies.Count - offset - 1; i++) {
			// calculate local energy average
			double currentEnergy = energies[i];
			double qwe = rangeSum(energies.ToArray(), i - offset, i - 1) + currentEnergy + rangeSum(energies.ToArray(), i + 1, i + offset);
			qwe /= offset * 2 + 1;

			// calculate energy variance of nearby energies
			List<double> nearbyEnergies = energies.Skip(i - 5).Take(5).Concat(energies.Skip(i + 1).Take(5)).ToList<double>();
			average = nearbyEnergies.Average();
			sumOfSquaresOfDifferences = nearbyEnergies.Sum(val => (val - average) * (val - average));
			variance = (sumOfSquaresOfDifferences / nearbyEnergies.Count) / Math.Pow(10, 22);

			// experimental linear regression - constant calculated according to local energy variance
			newC = variance * 0.009 + 1.385;
			if (currentEnergy > newC * qwe) beats++;
		}

		bpm = beats / (trackLength / 60);

	}

	private static double rangeQuadSum(double[] samples, int start, int stop) {
		double tmp = 0;
		for (int i = start; i <= stop; i++) {
			tmp += Math.Pow(samples[i], 2);
		}

		return tmp;
	}

	private static double rangeSum(double[] data, int start, int stop) {
		double tmp = 0;
		for (int i = start; i <= stop; i++) {
			tmp += data[i];
		}

		return tmp;
	}
}
