# Words frequency calculator

## Overview

  Program calculates frequencies of different words in input file and provides output in file in ordered by words.

## Assumptions

  Due to lack of time the following assumptions were made:
  * Words could be splitted only by the following 3 charachters:
    * ' '
    * \n
    * \r
  * Any symbol different from splitters considered as part of word.
  * Only positive unit tests implemented.
  * Output frequencies dictionary is small enough to live in memory.

## Project structure

Solution contains the following projects with classes:

* FrequencyDictionary
  
  Main console application.
  Reads arguments from command line.

  Usage:
  FrequencyDictionary.exe [input file] [output file]
  Input file should have Windows-1251 codepage.  

* FrequencyCalculationService
  
  Contains 3 implementations of calculation services:

  * SingleThreadedFrequencyCalculationService
    This implementation works in one thread for comparison with multithreaded implementations.

  * FrequencyCalculationService
    This implementation calculates frequencies in multiple threads then merges results of each processed block in one SortedDictionary.

    Contains the following classes:
      * DataAggregator
        Merges data from another dictionary to sorted dictionary.
      * FrequencyCalculator
        Calculates frequencies of words in input text and returns new dictionary with result.

  * NewFrequencyCalculationService
    This implementation calculates frequencies in multiple threads and updates result in concurrent dictionary. When all calclulations are finished dictionary is ordered. This is the fastest implementation and it is used in main console application.

    Contains the following classes:
    * FrequencyCalculatorWithUpdater
      Calculates frequencies of words in input string and updates result in input dictionary.

  Common classes for all implementations:
  * SequentialSplitter
    Retrieves words from string in IEnumerable.
      
  * StreamBlockReader
    Retrieves blocks from input stream with whole words. If word length is more than specified maximum then it will be splitted.

* FrequencyCalculationServiceTests

  Contains test data and unit tests for different parts of system.
  FrequencyCalculationServiceTests contains 3 tests for different implementation performance comparison.

## What else should be done
 * Additional IoC container should be added to cleanup constuctor parameters.
 * Logger should be added.
 * Negative and boundaries tests should be implemented.
 * Additional refactoring should be done:
   * Internal structure of FrequencyCalculationService should be optimized:
     * interfaces subfolder
     * different implementations should be moved to another assemblies
     * additional configurator of parameters like delimiters, maximum word length, maximum block size should be added
     * exceptions should be handled with adding information to log file, right now exceptions were not handled at all as time was spent on testing different approaches to find fastest prototype
 * Testing with file size bigger that PC memory should be done.
 * Read block size should be optimized.
 * File reading should be done in parallel with fixed block size, words cutted by boundaries should be collected with block numbers and result should be updated when neighbour parts will be collected.