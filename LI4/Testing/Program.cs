﻿using Testing;

const string ImageData = "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";
_ = new ImageCLI(ImageString.GetImage(ImageData));
