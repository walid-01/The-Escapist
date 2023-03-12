import sys
import joblib
import pandas as pd
from sklearn.preprocessing import MinMaxScaler

# Importing the trained model
model = joblib.load('modelPrediction.joblib')

# default min
minSession = [0,
              6,
              164,
              10,
              18,
              2,
              0,
              2,
              0,
              1,
              1110,
              1300]
# default max
maxSession = [1,
              53,
              761,
              134,
              534,
              57,
              20,
              14,
              2,
              49,
              4320,
              12500]

# getting data from c# and putting it in a list
val1 = float(sys.argv[1])
val2 = float(sys.argv[2])
val3 = float(sys.argv[3])
val4 = float(sys.argv[4])
val5 = float(sys.argv[5])
val6 = float(sys.argv[6])
val7 = float(sys.argv[7])
val8 = float(sys.argv[8])
val9 = float(sys.argv[9])
val10 = float(sys.argv[10])
val11 = float(sys.argv[11])
val12 = float(sys.argv[12])
row = [val1, val2, val3, val4, val5, val6, val7, val8, val9, val10, val11, val12]

# Creating a dataframe to use MinMax option
toDf = [row, minSession, maxSession]
df = pd.DataFrame(toDf)

# Scaling data to be used with the model
scaler = MinMaxScaler()
df[0:11] = scaler.fit_transform(df[0:11])

# Turning the result into a list
testValuesScaled = df.iloc[0].to_list()

model.predict([testValuesScaled])

print(model.predict([testValuesScaled])[0])
