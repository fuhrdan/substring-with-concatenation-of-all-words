//*****************************************************************************
//** 30. Substring With Concatenation of All Words                  leetcode **
//*****************************************************************************
// Define a large fixed-size hash map
#define HASH_SIZE 10000

typedef struct {
    int count;  // Expected count of the word
    int current; // Current count in the sliding window
} HashEntry;

// Hash function to map a word to an index
unsigned int hashWord(const char* word, int len) {
    unsigned int hash = 5381;
    for (int i = 0; i < len; i++) {
        hash = ((hash << 5) + hash) + word[i];
    }
    return hash % HASH_SIZE;
}

// Main function to find substring indices
int* findSubstring(char* s, char** words, int wordsSize, int* returnSize) {
    *returnSize = 0;
    if (!s || !words || wordsSize == 0) return NULL;

    int wordLen = strlen(words[0]);
    int totalLen = wordLen * wordsSize;
    int sLen = strlen(s);
    int* result = (int*)malloc(sLen * sizeof(int));

    if (sLen < totalLen) return result;

    // Create the hash table for word counts
    HashEntry wordMap[HASH_SIZE] = {0};
    for (int i = 0; i < wordsSize; i++) {
        unsigned int hash = hashWord(words[i], wordLen);
        wordMap[hash].count++;
    }

    // Sliding window for each possible offset
    for (int offset = 0; offset < wordLen; offset++) {
        HashEntry currentMap[HASH_SIZE] = {0};
        int count = 0;
        int start = offset;

        for (int end = offset; end + wordLen <= sLen; end += wordLen) {
            unsigned int hash = hashWord(s + end, wordLen);
            if (wordMap[hash].count > 0) {  // Valid word
                currentMap[hash].current++;
                count++;

                while (currentMap[hash].current > wordMap[hash].count) {
                    // Slide the window from the left
                    unsigned int startHash = hashWord(s + start, wordLen);
                    currentMap[startHash].current--;
                    start += wordLen;
                    count--;
                }

                if (count == wordsSize) {
                    result[(*returnSize)++] = start;
                }
            } else { // Invalid word, reset window
                memset(currentMap, 0, sizeof(currentMap));
                count = 0;
                start = end + wordLen;
            }
        }
    }

    return result;
}