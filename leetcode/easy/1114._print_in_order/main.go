package main

type Foo struct {
    ch1 chan struct{}
    ch2 chan struct{}
}

func NewFoo() *Foo {
	return &Foo{
        ch1: make(chan struct{}),
        ch2: make(chan struct{}),
	}
}

func (f *Foo) First(printFirst func()) {
	// Do not change this line
	printFirst()

    f.ch1 <- struct{}{}
}

func (f *Foo) Second(printSecond func()) {
    <- f.ch1

	/// Do not change this line
	printSecond()
    f.ch2 <- struct{}{}
}

func (f *Foo) Third(printThird func()) {
    <- f.ch2

	// Do not change this line
	printThird()
}