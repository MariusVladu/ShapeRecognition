# Shape Recognition
Shape recognition using Binary Synaptic Weights

Binary Synaptic Weights algorithm pseudocode
```
foreach outputClass
	outputNode = new OutputNode
	trainingSamplesNotProcessed = all training samples

	while(there are samples of this class not processed)

		averageVector = the average vector from trainingSamplesNotProcessed of this class
		
		key = the sample closest to the average vector
		yes = the furthest sample of this class
		no  = the closest sample of other classes
		
		group all trainingSamplesNotProcessed by their hamming distances to the key
		
		distance = 0
		do
			distance++
			samplesOfThisClass    = the count of trainingSamplesNotProcessed of this class at this distance from key
			samplesOfOtherClasses = the count of trainingSamplesNotProcessed of other classes at this distance from key
			
		while (samplesOfThisClass >= samplesOfOtherClasses && distance <= HammingDistance(key, yes))
		
		CreateSeparationPlane(distance - 1, key, outputNode) and set all samples with hamming distance <= distance as enclosed
		
		while (there is any enclosed sample with output class different than this output node's class)
			key = that sample
			group all trainingSamplesNotProcessed by their hamming distances to the key
			
			distance = 0
			do
				distance++
				samplesOfThisClass    = the count of trainingSamplesNotProcessed of this class at this distance from key
				samplesOfOtherClasses = the count of trainingSamplesNotProcessed of other classes at this distance from key
			while (samplesOfThisClassCount < samplesOfOtherClassesCount)
			
			CreateSeparationPlane(distance - 1, key, outputNode) and set all samples with hamming distance <= distance as enclosed
			remove key from enclosed samples
	
	update trainingSamplesNotProcessed
	
	outputNode.Threshold = the average number of separation planes needed for a pattern - 0.5
	add outputNode to outputNodes



CreateSeparationPlane(distance, key, outputNode)
	threshold = distance + 0.5 - (number of 1's in key)
	create a hidden node with the previously calculated threshold

	for i from 0 to key input vector's length
		create a link from inputNodes[i] to this hidden node with the weight:
			weight = 1 if key[i] == 0
			weight = -1 if key[i] == 1

	create a link from this hidden node to the outputNode
 ```
